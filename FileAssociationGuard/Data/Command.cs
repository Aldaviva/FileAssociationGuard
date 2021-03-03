namespace FileAssociationGuard.Data {

    public class Command {

        public string verb { get; }
        public string label { get; }
        public string command { get; }
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