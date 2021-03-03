using System.IO;
using Microsoft.Win32;

namespace FileAssociationGuard.Data {

    public readonly struct ApplicationPaths {

        public static readonly string? WINAMP                  = getAppPath("winamp.exe");
        public static readonly string? PHOTOSHOP               = getAppPath("Photoshop.exe");
        public static readonly string? ILLUSTRATOR             = getAppPath("Illustrator.exe");
        public static readonly string? VLC                     = getAppPath("vlc.exe");
        public static readonly string? DREAMWEAVER             = getAppPath("Dreamweaver.exe");
        public static readonly string? IRFANVIEW               = getInstallLocation(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IrfanView64", "i_view64.exe");
        public static readonly string? NOTEPAD2                = getInstallLocation(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Notepad2", "Notepad2.exe");
        public static readonly string? SUBLIME_TEXT            = getInstallLocation(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Sublime Text 3_is1", "sublime_text.exe");
        public static readonly string? XMLSPY                  = getAppPath("XMLSpy.exe");
        public static readonly string? VIVALDI                 = getInstallLocation(@"HKEY_CURRENT_USER\SOFTWARE\Vivaldi", "DestinationFolder", @"Application\vivaldi.exe");
        public static readonly string? VIVALDI_CUSTOM_LAUNCHER = getInstallLocation(@"HKEY_CURRENT_USER\SOFTWARE\Vivaldi", "DestinationFolder", "VivaldiCustomLauncher.exe");

        private static string? getAppPath(string exeFilename) {
            return Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + exeFilename, null, null) as string;
        }

        private static string? getInstallLocation(string uninstallKey, string valueName, string executableFilename) {
            return Registry.GetValue(uninstallKey, valueName, null) is string installLocation ? Path.Combine(installLocation, executableFilename) : null;
        }

        private static string? getInstallLocation(string uninstallKey, string executableFilename) {
            return getInstallLocation(uninstallKey, "InstallLocation", executableFilename);
        }

    }

}