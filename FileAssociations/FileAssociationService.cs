using System;
using System.Security.AccessControl;
using System.Security.Principal;
using FileAssociations.Data;
using Microsoft.Win32;

namespace FileAssociations {

    /// <summary>
    ///     See also https://kolbi.cz/blog/2017/10/25/setuserfta-userchoice-hash-defeated-set-file-type-associations-per-user/
    /// </summary>
    internal static class FileAssociationService {

        private const string DEFAULTICON = "DefaultIcon";
        private const string SHELL       = "shell";
        private const string COMMAND     = "command";
        private const string USERCHOICE  = "UserChoice";
        private const string ICON        = "Icon";

        private const bool DRY_RUN = false;

        private static readonly IdentityReference CURRENT_USER_IDENTITY = WindowsIdentity.GetCurrent().User!.Translate(typeof(SecurityIdentifier));

        public static void Main() {
            foreach (var association in Data.FileAssociations.ASSOCIATIONS) {
                applyFileAssociation(association);

                foreach (string extension in association.extensions) {
                    clearUserChoice(extension);
                }
            }
        }

        private static void applyFileAssociation(FileAssociation fileAssociation) {
            foreach (string extension in fileAssociation.extensions) {
                Console.WriteLine($"\nFixing file extension {extension}");

                regSetValue(@"HKEY_CLASSES_ROOT\" + extension, null, fileAssociation.programId);
            }

            using RegistryKey programKey = Registry.ClassesRoot.CreateSubKey(fileAssociation.programId);
            regSetValue(programKey, null, fileAssociation.label);

            using RegistryKey defaultIconKey = programKey.CreateSubKey(DEFAULTICON);
            regSetValue(defaultIconKey, null, fileAssociation.iconPath);

            if (!DRY_RUN) {
                programKey.DeleteSubKeyTree(SHELL, false);
            }

            using RegistryKey shellKey = programKey.CreateSubKey(SHELL);

            int commandIndex = 0;
            foreach (Command command in fileAssociation.commands.Compact()) {
                if (commandIndex == 0) {
                    regSetValue(shellKey, null, command.verb);
                }

                using RegistryKey verbKey = shellKey.CreateSubKey(command.verb);
                regSetValue(verbKey, null, command.label);

                if (command.icon != null) {
                    regSetValue(verbKey, ICON, command.icon);
                }

                using RegistryKey commandKey = verbKey.CreateSubKey(COMMAND);
                regSetValue(commandKey, null, command.command);

                commandIndex++;
            }
        }

        private static void regSetValue(RegistryKey key, string? value, object data) {
            Console.WriteLine($"SetValue {key}\\{value ?? "(Default)"} = {data}");
            if (!DRY_RUN) {
                key.SetValue(value, data);
            }
        }

        private static void regSetValue(string key, string? value, object data) {
            Console.WriteLine($"SetValue {key}\\{value ?? "(Default)"} = {data}");
            if (!DRY_RUN) {
                Registry.SetValue(key, value, data);
            }
        }

        private static void clearUserChoice(string extension) {
            using RegistryKey? fileExtKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\FileExts\" + extension,
                RegistryKeyPermissionCheck.ReadWriteSubTree);

            if (fileExtKey?.OpenSubKey(USERCHOICE, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ChangePermissions) is { } userChoice) {
                removeCurrentUserDenySetValuePermissions(userChoice);

                Console.WriteLine($"Deleting {fileExtKey.Name}\\{USERCHOICE}");
                if (!DRY_RUN) {
                    fileExtKey.DeleteSubKey(USERCHOICE);
                }
            }
        }

        private static void removeCurrentUserDenySetValuePermissions(RegistryKey key) {
            RegistrySecurity registrySecurity = key.GetAccessControl();

            var registryAccessRule = new RegistryAccessRule(CURRENT_USER_IDENTITY, RegistryRights.SetValue, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Deny);
            registrySecurity.RemoveAccessRuleSpecific(registryAccessRule);

            Console.WriteLine($"Removing permissions that deny SetValue rights to {CURRENT_USER_IDENTITY}");
            if (!DRY_RUN) {
                key.SetAccessControl(registrySecurity);
            }
        }

    }

}