namespace FileAssociations.Data {

    public class FileAssociation {

        /// <summary>File extensions with leading periods, like <c>.mp3</c>.</summary>
        public IEnumerable<string> extensions { get; }

        /// <summary>The ProgID of the file association, like <c>Winamp.File.MP3</c>.</summary>
        public string programId { get; }

        /// <summary>Absolute path to the icon file (<c>*.ico</c>) or the icon resource in a PE (DLL, EXE) file. For resources in PE files, you can append <c>,0</c> (the default) for the ordinal icon resource (decimal starting at 0), or you can also append <c>,-1</c> for the icon resouce's unique ID (decimal, as viewed with a tool like Resource Hacker). Omitting this suffix will use the first icon in the file (ordinal 0).</summary>
        public string iconPath { get; }

        /// <summary>The friendly name of the file type, which will appear in the Type column in Windows Explorer, such as <c>MPEG-1 Layer III</c>.</summary>
        public string label { get; }

        /// <summary>List of commands to associate with this file extension. <c>null</c> commands will not be included, to make it easier for commands for programs that aren't installed to not appear in a broken state.</summary>
        public IEnumerable<Command?> commands { get; }

        /// <summary></summary>
        /// <param name="extensions">File extensions with leading periods, like <c>.mp3</c>.</param>
        /// <param name="programId">The ProgID of the file association, like <c>Winamp.File.MP3</c>.</param>
        /// <param name="label">The friendly name of the file type, which will appear in the Type column in Windows Explorer.</param>
        /// <param name="iconPath">Absolute path to the icon file (<c>*.ico</c>) or the icon resource in a PE (DLL, EXE) file. For resources in PE files, you can append <c>,0</c> (the default) for the ordinal icon resource (decimal starting at 0), or you can also append <c>,-1</c> for the icon resouce's unique ID (decimal, as viewed with a tool like Resource Hacker). Omitting this suffix will use the first icon in the file (ordinal 0).</param>
        /// <param name="commands">List of verbs and commands to appear in this file type's context menu.</param>
        public FileAssociation(IEnumerable<string> extensions, string programId, string label, string iconPath, IEnumerable<Command?> commands) {
            this.extensions = extensions.Select(extension => extension.StartsWith('.') ? extension : '.' + extension);
            this.programId  = programId;
            this.iconPath   = iconPath;
            this.commands   = commands;
            this.label      = label;
        }

        public FileAssociation(string extension, string programId, string label, string iconPath, IEnumerable<Command?> commands): this(new[] { extension }, programId, label, iconPath, commands) { }

    }

}