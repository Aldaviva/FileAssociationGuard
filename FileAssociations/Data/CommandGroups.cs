using System.Collections.Generic;
using System.Linq;

namespace FileAssociations.Data {

    public readonly struct CommandGroups {

        public static readonly IEnumerable<Command?> AUDIO        = new[] { Commands.PLAY_AUDIO };
        public static readonly IEnumerable<Command?> VIDEO        = new[] { Commands.PLAY_VIDEO };
        public static readonly IEnumerable<Command?> RASTER_IMAGE = new[] { Commands.VIEW_IMAGE, Commands.EDIT_RASTER_IMAGE };
        public static readonly IEnumerable<Command?> VECTOR_IMAGE = new[] { Commands.VIEW_IMAGE, Commands.EDIT_VECTOR_IMAGE };
        public static readonly IEnumerable<Command?> TEXT         = new[] { Commands.EDIT_TEXT_NOTEPAD2, Commands.EDIT_TEXT_SUBLIME_TEXT };
        public static readonly IEnumerable<Command?> WEB_TEXT     = TEXT.Append(Commands.EDIT_TEXT_DREAMWEAVER);
        public static readonly IEnumerable<Command?> HTML         = new[] { Commands.BROWSE_WEB_PAGE }.Concat(WEB_TEXT);
        public static readonly IEnumerable<Command?> BAT          = new[] { Commands.EXECUTE_BAT }.Concat(TEXT);
        public static readonly IEnumerable<Command?> PS1          = new[] { Commands.EXECUTE_POWERSHELL, Commands.EDIT_POWERSHELL }.Concat(TEXT);

        public static readonly IEnumerable<Command?> PLAYLIST = new[] {
            Command.create("playWinamp", "Play in Winamp", Commands.PLAY_AUDIO?.command, Commands.PLAY_AUDIO?.icon),
            Command.create("playVlc", "Play in VLC", Commands.PLAY_VIDEO?.command, Commands.PLAY_VIDEO?.icon)
        };

    }

}