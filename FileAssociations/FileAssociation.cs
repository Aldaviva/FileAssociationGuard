using System.Collections.Generic;
using System.Linq;
using FileAssociations.Data;

namespace FileAssociations {

    public class FileAssociation {

        public IEnumerable<string> extensions { get; }
        public string programId { get; }
        public string iconPath { get; }
        public string label { get; }
        public IEnumerable<Command?> commands { get; }

        /// <summary>
        /// </summary>
        /// <param name="extensions">File extensions with leading periods, like <c>.mp3</c></param>
        /// <param name="programId">The ProgID of the file association, like <c>Winamp.File.MP3</c></param>
        /// <param name="label">The friendly name of the file type, which will appear in the Type column in Windows Explorer.</param>
        /// <param name="iconPath">
        ///     The absolute path to the file containing the type's icon. If the file has multiple icons, you can append a comma followed by the icon's index (starting from <c>0</c>, which is
        ///     the default).
        /// </param>
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