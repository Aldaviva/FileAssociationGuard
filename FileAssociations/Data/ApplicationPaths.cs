using Microsoft.Win32;

namespace FileAssociations.Data {

    public readonly struct ApplicationPaths {

        public static readonly string? WINAMP                  = getAppPath("winamp.exe");
        public static readonly string? PHOTOSHOP               = getAppPath("Photoshop.exe");
        public static readonly string? ILLUSTRATOR             = getAppPath("Illustrator.exe");
        public static readonly string? VLC                     = getAppPath("vlc.exe");
        public static readonly string? DREAMWEAVER             = getAppPath("Dreamweaver.exe");
        public static readonly string? BRIDGE                  = getAppPath("Bridge.exe");
        public static readonly string? IRFANVIEW               = getInstallLocation(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\IrfanView64", "i_view64.exe");
        public static readonly string? NOTEPAD2                = getInstallLocation(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Notepad2", "Notepad2.exe");
        public static readonly string? SUBLIME_TEXT            = getInstallLocation(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Sublime Text 3_is1", "sublime_text.exe");
        public static readonly string? XMLSPY                  = getAppPath("XMLSpy.exe");
        public static readonly string? VIVALDI                 = getInstallLocation(@"HKEY_CURRENT_USER\SOFTWARE\Vivaldi", "DestinationFolder", @"Application\vivaldi.exe");
        public static readonly string? VIVALDI_CUSTOM_LAUNCHER = getInstallLocation(@"HKEY_CURRENT_USER\SOFTWARE\Vivaldi", "DestinationFolder", "VivaldiCustomLauncher.exe");
        public static readonly string? TOTAL_COMMANDER         = getInstallLocation(@"HKEY_CURRENT_USER\SOFTWARE\Ghisler\Total Commander", "InstallDir", "totalcmd64.exe");

        /// <summary>
        ///     Determine the absolute path of an EXE file using <c>HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths</c>.
        /// </summary>
        /// <param name="exeFilename">The basename of the EXE file, with the file extension, e.g. <c>Photoshop.exe</c>.</param>
        /// <returns>The absolute path of the EXE file, or <c>null</c> if the EXE does not exist in <C>App Paths</C>.</returns>
        private static string? getAppPath(string exeFilename) {
            const string APP_PATHS_KEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\";
            return Registry.GetValue(Path.Combine(APP_PATHS_KEY, exeFilename), null, null) as string;
        }

        private static string? getInstallLocation(string uninstallKey, string executableFilename) {
            return getInstallLocation(uninstallKey, "InstallLocation", executableFilename);
        }

        private static string? getInstallLocation(string uninstallKey, string valueName, string executableFilename) {
            return Registry.GetValue(uninstallKey, valueName, null) is string installLocation ? Path.Combine(installLocation, executableFilename) : null;
        }

    }

}