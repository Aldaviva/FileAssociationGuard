namespace FileAssociationGuard.Data {

    /// <remarks>
    ///     <para>
    ///         When referring to a specific icon in a DLL or EXE, you can append <c>,x</c> to the filename, where <c>x</c> is the icon's identifier.
    ///         The value of <c>x</c> can be a non-negative integer, in which case it represents the ordinal position of the icon (starting from 0),
    ///         or it can be a negative integer, in which case it represents the resource ID as seen in tools like Resource Hacker.
    ///     </para>
    ///     <para>You don't need to put filenames in quotations marks, even if it contains spaces.</para>
    /// </remarks>
    public readonly struct Icons {

        /// <remarks>
        ///     <c>%SystemRoot%\System32\wmploc.dll</c> will be missing if you've uninstalled Windows Media Player using <c>OptionalFeatures.exe</c>. You also need
        ///     <c>%SystemRoot%\System32\en-US\wmploc.dll.mui</c> and <c>%SystemRoot%\SystemResources\wmploc.dll.mun</c> (which contains the actual icon data, and requires very elevated permissions to write to –
        ///     more than Administrator group membership, like TrustedInstaller).
        /// </remarks>
        private const string WMPLOC = @"%SystemRoot%\system32\wmploc.dll";

        private const string IMAGERES = @"%SystemRoot%\System32\imageres.dll";

        public static readonly string FLAC        = @"%SystemRoot%\Icons\flac.ico";
        public static readonly string M4A         = WMPLOC + ",-738";
        public static readonly string MP3         = WMPLOC + ",-732";
        public static readonly string OGG         = @"%SystemRoot%\Icons\ogg.ico";
        public static readonly string OPUS        = @"%SystemRoot%\Icons\opus.ico";
        public static readonly string WAV         = WMPLOC + ",-734";
        public static readonly string WINAMP_FILE = ApplicationPaths.WINAMP + ",1";
        public static readonly string WMA         = WMPLOC + ",-735";

        public static readonly string ASF           = @"%SystemRoot%\Icons\asf.ico";
        public static readonly string AVI           = WMPLOC + ",-731";
        public static readonly string FLV           = @"%SystemRoot%\Icons\flv.ico";
        public static readonly string M4V           = @"%SystemRoot%\Icons\m4v.ico";
        public static readonly string MKV           = @"%SystemRoot%\Icons\mkv.ico";
        public static readonly string MOV           = @"%SystemRoot%\Icons\mov.ico";
        public static readonly string MP4           = @"%SystemRoot%\Icons\mp4.ico";
        public static readonly string MPEG          = WMPLOC + ",-733";
        public static readonly string TS            = @"%SystemRoot%\Icons\ts.ico";
        public static readonly string VIDEO_GENERIC = WMPLOC + ",-730";
        public static readonly string WEBM          = @"%SystemRoot%\Icons\webm.ico";
        public static readonly string WMV           = WMPLOC + ",-736";

        public static readonly string AI            = ApplicationPaths.ILLUSTRATOR + ",1";
        public static readonly string CR3           = @"%SystemRoot%\Icons\cr3.ico";
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

        public static readonly string BAT  = IMAGERES + ",-68";
        public static readonly string CSS  = ApplicationPaths.DREAMWEAVER + ",8";
        public static readonly string HTML = ApplicationPaths.DREAMWEAVER + ",17";
        public static readonly string JS   = ApplicationPaths.DREAMWEAVER + ",7";
        public static readonly string JSON = ApplicationPaths.DREAMWEAVER + ",24";
        public static readonly string MD   = @"%SystemRoot%\Icons\markdown.ico";
        public static readonly string PHP  = ApplicationPaths.DREAMWEAVER + ",6";
        public static readonly string PS1  = @"""%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell_ise.exe"",1";
        public static readonly string TXT  = IMAGERES + ",-102";
        public static readonly string VBS  = @"%SystemRoot%\System32\WScript.exe,2";
        public static readonly string WSH  = @"%SystemRoot%\System32\WScript.exe,1";
        public static readonly string XML  = ApplicationPaths.DREAMWEAVER + ",9";

    }

}