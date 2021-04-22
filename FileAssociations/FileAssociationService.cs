using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using FileAssociations.Data;
using Microsoft.Win32;
using NLog;

namespace FileAssociations {

    /// <summary>If this doesn't work, try using https://kolbi.cz/blog/2017/10/25/setuserfta-userchoice-hash-defeated-set-file-type-associations-per-user/ </summary>
    internal class FileAssociationService {

        private static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        private const string DEFAULTICON            = "DefaultIcon";
        private const string SHELL                  = "shell";
        private const string COMMAND                = "command";
        private const string USERCHOICE             = "UserChoice";
        private const string ICON                   = "Icon";
        private const string SYSTEMFILEASSOCIATIONS = "SystemFileAssociations";
        private const string OPEN_WITH_LIST         = "OpenWithList";
        private const string FILE_EXTS              = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\";

        private readonly bool              isDryRun;
        private readonly IdentityReference currentUserIdentity;

        public FileAssociationService(bool isDryRun) {
            this.isDryRun       = isDryRun;
            currentUserIdentity = WindowsIdentity.GetCurrent().User!.Translate(typeof(SecurityIdentifier));
        }

        public void fixFileAssociations() {
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

            foreach (var association in FileAssociations.ASSOCIATIONS) {
                applyFileAssociation(association);

                foreach (string extension in association.extensions) {
                    using RegistryKey? fileExtKey = Registry.CurrentUser.OpenSubKey(FILE_EXTS + extension, true);
                    if (fileExtKey is not null) {
                        regDeleteSubKeyRecursively(fileExtKey, OPEN_WITH_LIST);
                    }

                    if (extension != ".htm" && extension != ".html") {
                        clearUserChoice(extension);
                    }
                }
            }

            foreach (string fileAssociationGroup in new[] { "text", "image", "audio", "video" }) {
                using RegistryKey? systemFileAssociationGroup = Registry.ClassesRoot.OpenSubKey($@"{SYSTEMFILEASSOCIATIONS}\{fileAssociationGroup}", true);
                if (systemFileAssociationGroup is not null) {
                    LOGGER.Debug($"Fixing system file association group {fileAssociationGroup}...");
                    regDeleteSubKeyRecursively(systemFileAssociationGroup, SHELL);
                }
            }
        }

        /// <exception cref="ValidationException">If the associations are invalid.</exception>
        private static void validate(IEnumerable<FileAssociation> associations) {
            foreach (FileAssociation fileAssociation in associations) {
                IEnumerable<IGrouping<string, Command>> commandsWithDuplicateVerbs = fileAssociation.commands.Compact().GroupBy(command => command.verb).Where(commands => commands.Count() > 1);
                foreach (IGrouping<string, Command> commands in commandsWithDuplicateVerbs) {
                    throw new ValidationException($"File association {fileAssociation.extensions.First()} has {commands.Count():N0} commands for duplicate verb \"{commands.Key}\":\n" +
                        string.Join('\n', commands.Select((command, i) => $" {i + 1:N0}. {command.label} ({command.command})")));
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

            int commandIndex = 0;
            foreach (Command command in fileAssociation.commands.Compact()) {
                if (commandIndex == 0) {
                    regSetValue(shellKey, null, command.verb);
                }

                using RegistryKey verbKey = shellKey.CreateSubKey(command.verb);
                regSetValue(verbKey, null, command.label);

                if (command.icon is not null) {
                    regSetValue(verbKey, ICON, command.icon);
                }

                using RegistryKey commandKey = verbKey.CreateSubKey(COMMAND);
                regSetValue(commandKey, null, command.command);

                commandIndex++;
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

        private void regSetValue(string key, string? value, object data) {
            LOGGER.Trace($"SetValue {key}\\{value ?? "(Default)"} = {data}");
            if (!isDryRun) {
                Registry.SetValue(key, value, data);
            }
        }

        private void clearUserChoice(string extension) {
            using RegistryKey? fileExtKey = Registry.CurrentUser.OpenSubKey(FILE_EXTS + extension, RegistryKeyPermissionCheck.ReadWriteSubTree);

            if (fileExtKey?.OpenSubKey(USERCHOICE, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ChangePermissions) is { } userChoice) {
                removeCurrentUserDenySetValuePermissions(userChoice);

                regDeleteSubKeyRecursively(fileExtKey, USERCHOICE);
            }
        }

        private void removeCurrentUserDenySetValuePermissions(RegistryKey key) {
            RegistrySecurity registrySecurity = key.GetAccessControl();

            RegistryAccessRule registryAccessRule = new(currentUserIdentity, RegistryRights.SetValue, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Deny);
            registrySecurity.RemoveAccessRuleSpecific(registryAccessRule);

            LOGGER.Trace($"Removing permissions that deny SetValue rights to {currentUserIdentity}");
            if (!isDryRun) {
                key.SetAccessControl(registrySecurity);
            }
        }

    }

}