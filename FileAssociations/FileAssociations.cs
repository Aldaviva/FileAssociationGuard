using FileAssociations.Data;

namespace FileAssociations {

    public readonly struct FileAssociations {

        public static readonly IEnumerable<FileAssociation> ASSOCIATIONS = new List<FileAssociation> {
            // Audio
            new(".flac", "Ben.Audio.FLAC", "Free Lossless Audio Codec", Icons.FLAC, CommandGroups.AUDIO),
            new(".m4a", "Ben.Audio.M4A", "MPEG-4 Audio", Icons.M4A, CommandGroups.AUDIO),
            new(".mp3", "Ben.Audio.MP3", "MPEG-1 Layer III", Icons.MP3, CommandGroups.AUDIO),
            new(".ogg", "Ben.Audio.OGG", "Ogg Vorbis", Icons.OGG, CommandGroups.AUDIO),
            new(".opus", "Ben.Audio.OPUS", "Opus", Icons.OPUS, CommandGroups.AUDIO),
            new(".wav", "Ben.Audio.WAV", "Waveform", Icons.WAV, CommandGroups.AUDIO),
            new(".wma", "Ben.Audio.WMA", "Windows Media Audio", Icons.WMA, CommandGroups.AUDIO),

            // Playlists
            new(new[] { ".m3u", ".m3u8" }, "Ben.Playlist.M3U", "M3U Playlist", Icons.WINAMP_FILE, CommandGroups.PLAYLIST),
            new(".pls", "Ben.Playlist.PLS", "Playlist", Icons.WINAMP_FILE, CommandGroups.PLAYLIST),

            // Images
            new(".ai", "Ben.Image.AI", "Illustrator Document", Icons.AI, CommandGroups.VECTOR_IMAGE),
            new(".bmp", "Ben.Image.BMP", "Bitmap", Icons.IMAGE_GENERIC, CommandGroups.RASTER_IMAGE),
            new(new[] { ".cr3", ".cr2" }, "Ben.Image.CR3", "Canon RAW 3", Icons.CR3, CommandGroups.RAW_IMAGE),
            new(".emf", "Ben.Image.EMF", "Enhanced Metafile", Icons.EMF, CommandGroups.VECTOR_IMAGE),
            new(".eps", "Ben.Image.EPS", "Encapsulated PostScript", Icons.EPS, CommandGroups.VECTOR_IMAGE),
            new(".gif", "Ben.Image.GIF", "Graphics Interchange Format", Icons.GIF, CommandGroups.RASTER_IMAGE),
            new(new[] { ".jpg", ".jpeg", ".jpe", ".jfif" }, "Ben.Image.JPEG", "Joint Photographic Experts Group", Icons.JPEG, CommandGroups.RASTER_IMAGE),
            new(new[] { ".jp2", ".j2k", ".jpf", ".jpm", ".jpg2", ".j2c", ".jpc", ".jpx" }, "Ben.Image.JPEG2000", "JPEG 2000", Icons.JPEG, CommandGroups.RASTER_IMAGE),
            new(".png", "Ben.Image.PNG", "Portable Network Graphics", Icons.PNG, CommandGroups.RASTER_IMAGE),
            new(".psd", "Ben.Image.PSD", "Photoshop Document", Icons.PSD, CommandGroups.RASTER_IMAGE),
            new(".svg", "Ben.Image.SVG", "Scalable Vector Graphics", Icons.SVG, CommandGroups.VECTOR_IMAGE),
            new(".tga", "Ben.Image.TGA", "TARGA", Icons.TGA, CommandGroups.RASTER_IMAGE),
            new(new[] { ".tif", ".tiff" }, "Ben.Image.TIFF", "Tagged Image File Format", Icons.TIFF, CommandGroups.RASTER_IMAGE),
            new(".webp", "Ben.Image.WEBP", "WebP", Icons.IMAGE_GENERIC, CommandGroups.RASTER_IMAGE),
            new(".wmf", "Ben.Image.WMF", "Windows Metafile", Icons.WMF, CommandGroups.VECTOR_IMAGE),

            // Video
            new(".asf", "Ben.Video.ASF", "Advanced Systems Format", Icons.ASF, CommandGroups.VIDEO),
            new(".avi", "Ben.Video.AVI", "Audio Video Interleave", Icons.AVI, CommandGroups.VIDEO),
            new(".flv", "Ben.Video.FLV", "Flash Video", Icons.FLV, CommandGroups.VIDEO),
            new(".mkv", "Ben.Video.MKV", "Matroska Video", Icons.MKV, CommandGroups.VIDEO),
            new(".mov", "Ben.Video.MOV", "QuickTime Movie", Icons.MOV, CommandGroups.VIDEO),
            new(".mp4", "Ben.Video.MP4", "MPEG-4 Video", Icons.MP4, CommandGroups.VIDEO),
            new(".m4v", "Ben.Video.M4V", "MPEG-4 Video", Icons.M4V, CommandGroups.VIDEO),
            new(new[] { ".mpg", ".mpeg" }, "Ben.Video.MPEG", "Motion Picture Experts Group", Icons.MPEG, CommandGroups.VIDEO),
            new(".ts", "Ben.Video.TS", "MPEG Transport Stream", Icons.TS, CommandGroups.VIDEO),
            new(".webm", "Ben.Video.WEBM", "WebM", Icons.WEBM, CommandGroups.VIDEO),
            new(".wmv", "Ben.Video.WMV", "Windows Media Video", Icons.WMV, CommandGroups.VIDEO),

            // Text
            new(new[] { ".txt", ".nfo", ".log" }, "Ben.Text.TXT", "Plain Text", Icons.TXT, CommandGroups.TEXT),
            new(".md", "Ben.Text.MD", "Markdown", Icons.MD, CommandGroups.TEXT),
            new(new[] { ".bat", ".cmd" }, "Ben.Text.BAT", "Batch File", Icons.BAT, CommandGroups.BAT),
            new(".ps1", "Ben.Text.PS1", "PowerShell Script", Icons.PS1, CommandGroups.PS1),
            new(".xml", "Ben.Text.XML", "Extensible Markup Language", Icons.XML, CommandGroups.XML),
            new(".json", "Ben.Text.JSON", "JavaScript Object Notation", Icons.JSON, CommandGroups.WEB_TEXT),
            new(".js", "Ben.Text.JS", "JavaScript", Icons.JS, CommandGroups.WEB_TEXT),
            new(new[] { ".css", ".less" }, "Ben.Text.CSS", "Cascading Style Sheet", Icons.CSS, CommandGroups.WEB_TEXT),
            new(new[] { ".html", ".htm" }, "Ben.Text.HTML", "Hypertext Markup Language", Icons.HTML, CommandGroups.HTML),
            new(".php", "Ben.Text.PHP", "PHP: Hypertext Preprocessor Script", Icons.PHP, CommandGroups.WEB_TEXT),
            new(".vbs", "Ben.Text.VBS", "Visual Basic Script", Icons.VBS, CommandGroups.TEXT),
            new(".wsh", "Ben.Text.WSH", "Windows Scripting Host Script", Icons.WSH, CommandGroups.TEXT),
            new(".reg", "Ben.Text.REG", "Registry Entries", Icons.REG, CommandGroups.REG),
            new(new[] { ".ini", ".conf", ".cfg", ".config", ".cnf" }, "Ben.Text.INI", "Configuration", Icons.INI, CommandGroups.TEXT),
        };

    }

}