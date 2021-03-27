namespace FileAssociations.Data {

    public class Command {

        /// <summary>
        ///     The unique string that acts as a key for this command, such as <c>open</c>.
        /// </summary>
        public string verb { get; }

        /// <summary>
        ///     The human-readable string that appears in the context menu, such as <c>Open</c>.
        /// </summary>
        public string label { get; }

        /// <summary>
        ///     The command-line invocation of a program which may contain <c>%</c> placeholders (such as <c>%1</c>, <c>%*</c>, or <c>%SYSTEMROOT%</c>), such as <c>"c:\myprogram.exe" "%1"</c>.
        /// </summary>
        public string command { get; }

        /// <summary>
        ///     Absolute path to the icon file (<c>*.ico</c>) or the icon resource in a PE (DLL, EXE) file. For resources in PE files, you can append <c>,0</c> (the default) for the ordinal icon resource (decimal starting at 0), or you can also append <c>,-1</c> for the icon resouce's unique ID (decimal, as viewed with a tool like Resource Hacker). Omitting this suffix will use the first icon in the file (ordinal 0).
        /// </summary>
        public string? icon { get; }

        private Command(string verb, string label, string command, string? icon) {
            this.verb    = verb;
            this.label   = label;
            this.command = command;
            this.icon    = icon;
        }

        private Command(string verb, string label, string applicationPath): this(verb, label, simpleCommand(applicationPath), applicationPath) { }

        public static string simpleCommand(string applicationPath) {
            return $"\"{applicationPath}\" \"%1\"";
        }

        public static Command? create(string verb, string label, string? applicationPath) {
            return applicationPath != null ? new Command(verb, label, applicationPath) : null;
        }

        public static Command? create(string verb, string label, string? command, string? icon) {
            return command != null ? new Command(verb, label, command, icon) : null;
        }

    }

}