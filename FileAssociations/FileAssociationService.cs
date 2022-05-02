using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using FileAssociations.Data;
using Microsoft.Win32;
using NLog;

namespace FileAssociations; 

internal class FileAssociationService {

    private static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

    private const string DEFAULTICON            = "DefaultIcon";
    private const string SHELL                  = "shell";
    private const string COMMAND                = "command";
    private const string ICON                   = "Icon";
    private const string SYSTEMFILEASSOCIATIONS = "SystemFileAssociations";
    private const string OPEN_WITH_LIST         = "OpenWithList";
    private const string FILE_EXTS              = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\";

    private readonly bool isDryRun;

    private bool hasTakeOwnershipPermission;

    public FileAssociationService(bool isDryRun) {
        this.isDryRun = isDryRun;
    }

    /// <exception cref="ValidationException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task fixFileAssociations() {
        try {
            validate(FileAssociations.ASSOCIATIONS);
        } catch (ValidationException e) {
            LOGGER.Error("File associations are invalid, exiting without applying any changes.");
            LOGGER.Error(e.Message);
            throw;
        }

        if (isDryRun) {
            LOGGER.Info("Dry-run mode. Changes below will be previewed but not applied.");
        }

        foreach (FileAssociation association in FileAssociations.ASSOCIATIONS) {
            applyFileAssociation(association);

            foreach (string extension in association.extensions) {
                using RegistryKey? fileExtKey = Registry.CurrentUser.OpenSubKey(FILE_EXTS + extension, true);
                if (fileExtKey is not null) {
                    regDeleteSubKeyRecursively(fileExtKey, OPEN_WITH_LIST);
                }
            }
        }

        foreach (string mediaType in new[] { "text", "image", "audio", "video" }) {
            using RegistryKey? mediaTypeKey = Registry.ClassesRoot.OpenSubKey($@"{SYSTEMFILEASSOCIATIONS}\{mediaType}", true);
            if (mediaTypeKey is not null) {
                LOGGER.Debug($"Fixing system file association for media type {mediaType}");
                regDeleteSubKeyRecursively(mediaTypeKey, SHELL);
            }
        }

        await applyUserChoices(FileAssociations.ASSOCIATIONS);

        fixFolderShellActions();

        await Icons.install();
    }

    /// <exception cref="ValidationException">If the associations are invalid.</exception>
    private static void validate(IEnumerable<FileAssociation> associations) {
        ISet<string> extensions = new HashSet<string>();
        foreach (FileAssociation association in associations) {
            foreach (string extension in association.extensions) {
                if (!Regex.IsMatch(extension, @"^\.[\w-]+$", RegexOptions.IgnoreCase)) {
                    throw new ValidationException($"Invalid extension {extension} for ProgID {association.programId}");
                }

                bool wasNew = extensions.Add(extension);
                if (!wasNew) {
                    throw new ValidationException($"Duplicate file extension {extension} in ProgID {association.programId}");
                }
            }
        }
    }

    private void applyFileAssociation(FileAssociation fileAssociation) {
        foreach (string extension in fileAssociation.extensions) {
            LOGGER.Debug($"Fixing file extension {extension}");

            regSetValue($"{Registry.ClassesRoot.Name}\\{extension}", null, fileAssociation.programId);

            using RegistryKey? systemFileAssociation = Registry.ClassesRoot.OpenSubKey($"{SYSTEMFILEASSOCIATIONS}\\{extension}", true);
            if (systemFileAssociation is not null) {
                regDeleteSubKeyRecursively(systemFileAssociation, SHELL);
            }
        }

        using RegistryKey programKey = Registry.ClassesRoot.CreateSubKey(fileAssociation.programId);
        regSetValue(programKey, null, fileAssociation.label);

        using RegistryKey defaultIconKey = programKey.CreateSubKey(DEFAULTICON);
        regSetValue(defaultIconKey, null, fileAssociation.iconPath);

        regDeleteSubKeyRecursively(programKey, SHELL);

        using RegistryKey shellKey = programKey.CreateSubKey(SHELL);

        int          commandIndex = 0;
        ISet<string> usedVerbs    = new HashSet<string>();
        foreach (Command command in fileAssociation.commands.Compact()) {
            /*
             * The first, default, command is always given the verb "open" even if a different verb was specified, because lots of Windows programs assume this always exists, like TagScanner.
             * Duplicate verbs are fixed by adding an incrementing suffix, which would result in an example command list with verbs ["open", "open", "open"] being converted to ["open", "open2", "open3"].
             */
            string verb = command.verb;
            if (commandIndex == 0) {
                verb = Commands.VERB_OPEN;
                regSetValue(shellKey, null, verb);
            } else {
                string nonCollidingVerb = verb;
                for (int suffix = 2; usedVerbs.Contains(nonCollidingVerb); suffix++) {
                    nonCollidingVerb = verb + suffix;
                }

                verb = nonCollidingVerb;
            }

            using RegistryKey verbKey = shellKey.CreateSubKey(verb);
            regSetValue(verbKey, null, command.label);

            if (command.icon is not null) {
                regSetValue(verbKey, ICON, command.icon);
            }

            using RegistryKey commandKey = verbKey.CreateSubKey(COMMAND);
            regSetValue(commandKey, null, command.command);

            commandIndex++;
            usedVerbs.Add(verb);
        }
    }

    /// <exception cref="InvalidOperationException"></exception>
    private async Task applyUserChoices(IEnumerable<FileAssociation> fileAssociations) {
        const string SET_USER_FTA_RESOURCE_NAME = "SetUserFTA.exe";
        string       setUserFtaExeFileName      = Path.Combine(Path.GetTempPath(), "SetUserFTA.exe");

        await using (Stream setUserFtaExeMemoryStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(SET_USER_FTA_RESOURCE_NAME) ??
                     throw new InvalidOperationException($"Could not find embedded resource named {SET_USER_FTA_RESOURCE_NAME}"))
        await using (FileStream setUserFtaExeFileStream = File.Open(setUserFtaExeFileName, FileMode.Create, FileAccess.Write)) {
            await setUserFtaExeMemoryStream.CopyToAsync(setUserFtaExeFileStream);
        }

        LOGGER.Trace("Saved {0}", setUserFtaExeFileName);

        string associationsFileName = Path.GetTempFileName();

        await using (TextWriter associationsFileWriter = new StreamWriter(associationsFileName, false, Encoding.UTF8)) {
            foreach (FileAssociation fileAssociation in fileAssociations) {
                foreach (string fileExtension in fileAssociation.extensions) {
                    await associationsFileWriter.WriteLineAsync(string.Join(", ", fileExtension, fileAssociation.programId));
                }
            }

            await associationsFileWriter.FlushAsync();
        }

        LOGGER.Trace("Saved argument file for SetUserFTA.exe to {0}", associationsFileName);

        if (!isDryRun) {
            using Process setUserFtaProcess = Process.Start(setUserFtaExeFileName, associationsFileName) ?? throw new InvalidOperationException($"Failed to start {setUserFtaExeFileName}");
            await setUserFtaProcess.WaitForExitAsync();
        }

        LOGGER.Info("Set User Choices.");
        File.Delete(setUserFtaExeFileName);
        File.Delete(associationsFileName);
    }

    private void fixFolderShellActions() {
        foreach (string subkeyName in new[] { "cmd", "PowerShell", "Total Commander" }) {
            LOGGER.Info($"Fixing directory shell command {subkeyName}");
            foreach (string keyName in new[] { "Background", string.Empty }) {
                string      subkey = Path.Combine("Directory", keyName, SHELL, subkeyName);
                RegistryKey key;

                try {
                    key = Registry.ClassesRoot.CreateSubKey(subkey, true);
                } catch (UnauthorizedAccessException) {
                    regTakeOwnership(Registry.ClassesRoot, subkey);
                    key = Registry.ClassesRoot.CreateSubKey(subkey, true);
                }

                using (key) {
                    regDeleteValue(key, "Extended");

                    switch (subkeyName) {
                        case "cmd":
                            regDeleteValue(key, "HideBasedOnVelocityId");
                            regSetValue(key, null, "Open in Command Prompt");
                            regSetValue(key, ICON, Environment.ExpandEnvironmentVariables(@"%SYSTEMROOT%\System32\cmd.exe,0"));
                            break;
                        case "PowerShell":
                            regDeleteValue(key, "ShowBasedOnVelocityId");
                            regSetValue(key, null, "Open in PowerShell");
                            regSetValue(key, ICON, Environment.ExpandEnvironmentVariables(@"%SYSTEMROOT%\System32\WindowsPowerShell\v1.0\powershell.exe,0"));
                            break;
                        case "Total Commander":
                            regSetValue(key, null, "Open in Total Commander");
                            //separate ICO extracted from (32-bit) totalcmd.exe,0 because it looks nicer than totalcmd64.exe,0 and is higher resolution than tcusbrun.exe,0
                            regSetValue(key, ICON, Path.Combine(Path.GetDirectoryName(ApplicationPaths.TOTAL_COMMANDER)!, "MAINICON.ico"));
                            using (RegistryKey commandKey = key.CreateSubKey(COMMAND, true)) {
                                commandKey.SetValue(null, $"\"{ApplicationPaths.TOTAL_COMMANDER}\" /o \"%V\"");
                            }

                            break;
                    }
                }
            }
        }
    }

    private void regDeleteSubKeyRecursively(RegistryKey parentKey, string childNameToDelete) {
        LOGGER.Trace($"Deleting {parentKey}\\{childNameToDelete}");
        if (!isDryRun) {
            parentKey.DeleteSubKeyTree(childNameToDelete, false);
        }
    }

    private void regSetValue(RegistryKey key, string? value, object data) {
        LOGGER.Trace($"SetValue {key}\\{value ?? "(Default)"} = {data}");
        if (!isDryRun) {
            key.SetValue(value, data);
        }
    }

    private void regDeleteValue(RegistryKey key, string? value) {
        LOGGER.Trace($"DeleteValue {key}\\{value ?? "(Default)"}");
        if (!isDryRun) {
            if (value is not null) {
                key.DeleteValue(value, false);
            } else {
                key.SetValue(value, string.Empty);
            }
        }
    }

    private void regSetValue(string key, string? value, object data) {
        LOGGER.Trace($"SetValue {key}\\{value ?? "(Default)"} = {data}");
        if (!isDryRun) {
            Registry.SetValue(key, value, data);
        }
    }

    private void regTakeOwnership(RegistryKey hive, string key) {
        if (!hasTakeOwnershipPermission) {
            SecurityTokenManipulator.AddPrivilege(SecurityTokenManipulator.SE_RESTORE_NAME);
            SecurityTokenManipulator.AddPrivilege(SecurityTokenManipulator.SE_TAKE_OWNERSHIP_NAME);
            hasTakeOwnershipPermission = true;
        }

        LOGGER.Trace($"Taking ownership of {hive}\\{key}");

        //BuiltinAdministratorsSid is the administrators group, not the administrator user, contrary to what the documentation says
        SecurityIdentifier adminGroup = new(WellKnownSidType.BuiltinAdministratorsSid, WindowsIdentity.GetCurrent().User!.AccountDomainSid);

        using (RegistryKey keyHandle = hive.OpenSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.TakeOwnership)!) {
            RegistrySecurity registrySecurity = keyHandle.GetAccessControl();
            registrySecurity.SetOwner(adminGroup);

            if (!isDryRun) {
                keyHandle.SetAccessControl(registrySecurity);
            }
        }

        if (!isDryRun) {
            using RegistryKey keyHandle        = hive.OpenSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ChangePermissions)!;
            RegistrySecurity  registrySecurity = keyHandle.GetAccessControl();
            registrySecurity.AddAccessRule(new RegistryAccessRule(adminGroup, RegistryRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                PropagationFlags.None, AccessControlType.Allow));

            keyHandle.SetAccessControl(registrySecurity);
        }
    }

}