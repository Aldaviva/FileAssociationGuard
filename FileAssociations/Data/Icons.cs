// ReSharper disable ConvertToConstant.Global

using System.Reflection;
using NLog;

namespace FileAssociations.Data; 

/// <remarks>
///     <para>
///         When referring to a specific icon in a DLL or EXE, you can append <c>,x</c> to the filename, where <c>x</c> is the icon's identifier.
///         The value of <c>x</c> can be a non-negative integer, in which case it represents the ordinal position of the icon (starting from 0),
///         or it can be a negative integer, in which case it represents the resource ID as seen in tools like Resource Hacker. If you don't
///         append a specifier, the first icon in the file (index 0) is used.
///     </para>
///     <para>You don't need to put filenames in quotations marks, even if it contains spaces.</para>
///     <para>
///         Some of these files were ripped from <c>%SystemRoot%\SystemResources\wmploc.dll.mun</c> (because it seems to not load correctly in 32-bit
///         Total Commander due to WoW64 filesystem redirection). These include M4A and M4V (which I also edited because the 256px images had the wrong
///         vertical position), MP3, WAV, WMA, AVI, MPEG, WMV, and the generic video icon.
///     </para>
///     <para>
///         I forged other ones to look like the official icons and expand them to handle more file types, including FLAC, OGG, OPUS, ASF, FLV, M4V,
///         MKV, MOV, MP4, TS, WEBM, and CR3.
///     </para>
/// </remarks>
public readonly struct Icons {

    private static readonly Logger LOGGER = LogManager.GetLogger(nameof(Icons));

    private const string IMAGERES  = @"%SystemRoot%\System32\imageres.dll";
    private const string ICONS_DIR = @"%SystemRoot%\Icons\";

    // Audio
    public static readonly string FLAC        = ICONS_DIR + "flac.ico";
    public static readonly string M4A         = ICONS_DIR + "m4a.ico";
    public static readonly string MP3         = ICONS_DIR + "mp3.ico";
    public static readonly string OGG         = ICONS_DIR + "ogg.ico";
    public static readonly string OPUS        = ICONS_DIR + "opus.ico";
    public static readonly string WAV         = ICONS_DIR + "wav.ico";
    public static readonly string WINAMP_FILE = ApplicationPaths.WINAMP + ",1";
    public static readonly string WMA         = ICONS_DIR + "wma.ico";

    // Video
    public static readonly string ASF           = ICONS_DIR + "asf.ico";
    public static readonly string AVI           = ICONS_DIR + "avi.ico";
    public static readonly string FLV           = ICONS_DIR + "flv.ico";
    public static readonly string M4V           = ICONS_DIR + "m4v.ico";
    public static readonly string MKV           = ICONS_DIR + "mkv.ico";
    public static readonly string MOV           = ICONS_DIR + "mov.ico";
    public static readonly string MP4           = ICONS_DIR + "mp4.ico";
    public static readonly string MPEG          = ICONS_DIR + "mpeg.ico";
    public static readonly string TS            = ICONS_DIR + "ts.ico";
    public static readonly string VIDEO_GENERIC = ICONS_DIR + "video_generic.ico";
    public static readonly string WEBM          = ICONS_DIR + "webm.ico";
    public static readonly string WMV           = ICONS_DIR + "wmv.ico";

    // Images
    public static readonly string AI            = ApplicationPaths.ILLUSTRATOR + ",1";
    public static readonly string CR3           = ICONS_DIR + "cr3.ico";
    public static readonly string EMF           = ApplicationPaths.ILLUSTRATOR + ",14";
    public static readonly string EPS           = ApplicationPaths.ILLUSTRATOR + ",2";
    public static readonly string GIF           = IMAGERES + ",-71";
    public static readonly string IMAGE_GENERIC = IMAGERES + ",-70";
    public static readonly string JPEG          = IMAGERES + ",-72";
    public static readonly string PNG           = IMAGERES + ",-83";
    public static readonly string PSD           = ApplicationPaths.PHOTOSHOP + ",1";
    public static readonly string SVG           = ApplicationPaths.ILLUSTRATOR + ",16";
    public static readonly string TGA           = ApplicationPaths.PHOTOSHOP + ",19";
    public static readonly string TIFF          = ApplicationPaths.PHOTOSHOP + ",5";
    public static readonly string WMF           = ApplicationPaths.ILLUSTRATOR + ",18";

    //Text
    public static readonly string BAT  = IMAGERES + ",-68";
    public static readonly string CSS  = ICONS_DIR + "css.ico";
    public static readonly string LESS = ICONS_DIR + "less.ico";
    public static readonly string HTML = ICONS_DIR + "html.ico";
    public static readonly string JS   = ICONS_DIR + "js.ico";
    public static readonly string JSON = ICONS_DIR + "json.ico";
    public static readonly string MD   = ICONS_DIR + "markdown.ico";
    public static readonly string PHP  = ICONS_DIR + "php.ico";
    public static readonly string PS1  = @"%SYSTEMROOT%\System32\WindowsPowerShell\v1.0\powershell.exe";
    public static readonly string REG  = @"%SystemRoot%\regedit.exe,1";
    public static readonly string TXT  = IMAGERES + ",-102";
    public static readonly string INI  = IMAGERES + ",-69";
    public static readonly string VBS  = @"%SystemRoot%\System32\WScript.exe,2";
    public static readonly string WSH  = @"%SystemRoot%\System32\WScript.exe,1";
    public static readonly string XML  = ICONS_DIR + "xml.ico";

    public static Task install() {
        Assembly assembly = Assembly.GetExecutingAssembly();

        return Task.WhenAll(from resourceName in assembly.GetManifestResourceNames()
            let iconNamePrefix = $"{assembly.GetName().Name}.Resources.Icons."
            where resourceName.StartsWith(iconNamePrefix)
            let filename = resourceName[iconNamePrefix.Length..]
            let absolutePath = Environment.ExpandEnvironmentVariables(Path.Combine(ICONS_DIR, filename))
            let manifestResourceStream = assembly.GetManifestResourceStream(resourceName)!
            select ((Func<Task>) (() => {
                try {
                    using FileStream fileStream = File.Open(absolutePath, FileMode.CreateNew, FileAccess.Write);
                    LOGGER.Debug($"Copying icon to {absolutePath}");
                    return manifestResourceStream.CopyToAsync(fileStream);
                } catch (IOException) {
                    return Task.CompletedTask;
                    // File already exists, leave it along
                }
            }))());
    }

}