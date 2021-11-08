using System;
using System.Diagnostics.CodeAnalysis;

namespace FileAssociations.Data {

    public class Command {

        /// <summary>The unique string that acts as a key for this command, such as <c>open</c>.</summary>
        public string verb { get; }

        /// <summary>The human-readable string that appears in the context menu, such as <c>Open</c>.</summary>
        public string label { get; }

        /// <summary>The command-line invocation of a program which may contain <c>%</c> placeholders (such as <c>%1</c> or <c>%*</c>), such as <c>"c:\myprogram.exe" "%1"</c>.
        /// Environment variables don't work because Windows will throw a Command Not Found error, so we pre-expand them when creating this object.</summary>
        public string command { get; }

        /// <summary>Absolute path to the icon file (<c>*.ico</c>) or the icon resource in a PE (DLL, EXE) file. For resources in PE files, you can append <c>,0</c> (the default) for the ordinal icon resource (decimal starting at 0), or you can also append <c>,-1</c> for the icon resouce's unique ID (decimal, as viewed with a tool like Resource Hacker). Omitting this suffix will use the first icon in the file (ordinal 0). Environment variables such as <c>%SYSTEMROOT%</c> are allowed here, unlike in <c>command</c>.</summary>
        public string? icon { get; }

        private Command(string verb, string label, string command, string? icon) {
            this.verb    = verb;
            this.label   = label;
            this.command = Environment.ExpandEnvironmentVariables(command); // breaks with %SYSTEMROOT% for .ps1 files, works with C:\Windows\
            this.icon    = icon;
        }

        private Command(string verb, string label, string applicationPath): this(verb, label, simpleCommand(applicationPath), applicationPath) { }

        private static string simpleCommand(string applicationPath) {
            return $"\"{applicationPath}\" \"%1\"";
        }

        public static Command? create(string verb, string label, string? applicationPath) {
            return applicationPath is not null ? new Command(verb, label, applicationPath) : null;
        }

        [return: NotNullIfNotNull(nameof(command))]
        public static Command? create(string verb, string label, string? command, string? icon) {
            return command is not null ? new Command(verb, label, command, icon) : null;
        }

    }

}