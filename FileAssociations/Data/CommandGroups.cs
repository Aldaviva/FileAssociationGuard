namespace FileAssociations.Data; 

public readonly struct CommandGroups {

    public static readonly IEnumerable<Command?> AUDIO        = new[] { Commands.PLAY_AUDIO };
    public static readonly IEnumerable<Command?> VIDEO        = new[] { Commands.PLAY_VIDEO };
    public static readonly IEnumerable<Command?> RASTER_IMAGE = new[] { Commands.VIEW_IMAGE, Commands.EDIT_RASTER_IMAGE };
    public static readonly IEnumerable<Command?> RAW_IMAGE    = RASTER_IMAGE.Append(Commands.BROWSE_IMAGE);
    public static readonly IEnumerable<Command?> VECTOR_IMAGE = new[] { Commands.EDIT_VECTOR_IMAGE, Commands.EDIT_RASTER_IMAGE?.with("Edit in Photoshop", "editPhotoshop") };
    public static readonly IEnumerable<Command?> TEXT         = new[] { Commands.EDIT_TEXT_NOTEPAD2, Commands.EDIT_TEXT_SUBLIME_TEXT };
    public static readonly IEnumerable<Command?> WEB_TEXT     = TEXT.Append(Commands.EDIT_TEXT_DREAMWEAVER);
    public static readonly IEnumerable<Command?> XML          = WEB_TEXT.Append(Commands.EDIT_TEXT_XMLSPY);
    public static readonly IEnumerable<Command?> HTML         = new[] { Commands.BROWSE_WEB_PAGE }.Concat(WEB_TEXT);
    public static readonly IEnumerable<Command?> BAT          = new[] { Commands.EXECUTE_BAT }.Concat(TEXT);
    public static readonly IEnumerable<Command?> PS1          = new[] { Commands.EXECUTE_POWERSHELL }.Concat(TEXT);
    public static readonly IEnumerable<Command?> REG          = TEXT.Append(Commands.MERGE_REGISTRY);
    public static readonly IEnumerable<Command?> PLAYLIST     = new[] { Commands.PLAY_AUDIO?.with("Play in Winamp"), Commands.PLAY_VIDEO?.with("Play in VLC", "playVlc") };

}