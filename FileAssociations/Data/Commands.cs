namespace FileAssociations.Data {

    public readonly struct Commands {

        public static readonly Command? PLAY_AUDIO             = Command.create("open", "Play", ApplicationPaths.WINAMP);
        public static readonly Command? VIEW_IMAGE             = Command.create("open", "View", ApplicationPaths.IRFANVIEW); //verb must be "open" for TagScanner "View embedded cover" button to work
        public static readonly Command? EDIT_RASTER_IMAGE      = Command.create("edit", "Edit", ApplicationPaths.PHOTOSHOP);
        public static readonly Command? EDIT_VECTOR_IMAGE      = Command.create("edit", "Edit", ApplicationPaths.ILLUSTRATOR);
        public static readonly Command? BROWSE_IMAGE           = Command.create("browse", "Browse", ApplicationPaths.BRIDGE);
        public static readonly Command? PLAY_VIDEO             = Command.create("open", "Play", ApplicationPaths.VLC);
        public static readonly Command? EDIT_TEXT_NOTEPAD2     = Command.create("editNotepad2", "Edit with Notepad2", ApplicationPaths.NOTEPAD2);
        public static readonly Command? EDIT_TEXT_SUBLIME_TEXT = Command.create("editSublimeText", "Edit with Sublime Text", ApplicationPaths.SUBLIME_TEXT);
        public static readonly Command? EDIT_TEXT_DREAMWEAVER  = Command.create("editDreamweaver", "Edit with Dreamweaver", ApplicationPaths.DREAMWEAVER);
        public static readonly Command? EDIT_TEXT_XMLSPY       = Command.create("editXmlspy", "Edit with XMLSpy", ApplicationPaths.XMLSPY);
        public static readonly Command? EXECUTE_BAT            = Command.create("open", "Run", @"""%1"" %*", @"%SYSTEMROOT%\System32\cmd.exe");
        public static readonly Command? BROWSE_WEB_PAGE        = Command.create("open", "Browse", $@"""{ApplicationPaths.VIVALDI_CUSTOM_LAUNCHER}"" -- ""%1""", ApplicationPaths.VIVALDI);

        public static readonly Command? EDIT_POWERSHELL = Command.create("editPowerShellIse", "Edit with PowerShell ISE", @"""%SYSTEMROOT%\System32\WindowsPowerShell\v1.0\powershell_ise.exe"" ""%1""",
            @"%SYSTEMROOT%\System32\WindowsPowerShell\v1.0\powershell_ise.exe");

        public static readonly Command? EXECUTE_POWERSHELL = Command.create("open", "Run",
            @"""%SYSTEMROOT%\System32\WindowsPowerShell\v1.0\powershell.exe"" ""-Command"" ""if((Get-ExecutionPolicy ) -ne 'AllSigned') { Set-ExecutionPolicy -Scope Process Bypass }; & '%1'""",
            @"%SYSTEMROOT%\System32\WindowsPowerShell\v1.0\powershell.exe"); //from HKEY_CLASSES_ROOT\SystemFileAssociations\.ps1\Shell\0\Command

    }

}