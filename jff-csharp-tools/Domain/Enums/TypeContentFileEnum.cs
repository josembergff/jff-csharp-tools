using System.ComponentModel;

namespace JffCsharpTools.Domain.Enums
{
    /// <summary>
    /// Enumeration that defines various file content types with their corresponding MIME types.
    /// Each enum value represents a specific file format and includes a Description attribute
    /// containing the standard MIME type for that format.
    /// </summary>
    public enum TypeContentFileEnum
    {
        // Document and text formats
        /// <summary>
        /// Comma-separated values file format
        /// </summary>
        // Document and text formats
        /// <summary>
        /// Comma-separated values file format
        /// </summary>
        [Description("text/csv")]
        CSV = 1,
        /// <summary>
        /// Plain text file format
        /// </summary>
        [Description("text/plain")]
        TXT = 2,
        /// <summary>
        /// JavaScript Object Notation format
        /// </summary>
        [Description("application/json")]
        JSON = 3,
        /// <summary>
        /// eXtensible Markup Language format
        /// </summary>
        [Description("application/xml")]
        XML = 4,
        /// <summary>
        /// Portable Document Format
        /// </summary>
        [Description("application/pdf")]
        PDF = 5,
        /// <summary>
        /// Microsoft Excel Open XML spreadsheet format
        /// </summary>
        [Description("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        XLSX = 6,
        /// <summary>
        /// Microsoft Word Open XML document format
        /// </summary>
        [Description("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        DOCX = 7,
        /// <summary>
        /// Microsoft PowerPoint Open XML presentation format
        /// </summary>
        [Description("application/vnd.openxmlformats-officedocument.presentationml.presentation")]
        PPTX = 8,

        // Image formats
        /// <summary>
        /// JPEG image format
        /// </summary>
        [Description("image/jpeg")]
        JPEG = 9,
        /// <summary>
        /// Portable Network Graphics image format
        /// </summary>
        [Description("image/png")]
        PNG = 10,
        /// <summary>
        /// Graphics Interchange Format image
        /// </summary>
        [Description("image/gif")]
        GIF = 11,

        // Media formats
        /// <summary>
        /// MPEG-4 video format
        /// </summary>
        [Description("video/mp4")]
        MP4 = 12,
        /// <summary>
        /// MPEG-1 Audio Layer III audio format
        /// </summary>
        [Description("audio/mpeg")]
        MP3 = 13,

        // Archive and compression formats
        /// <summary>
        /// ZIP archive format
        /// </summary>
        [Description("application/zip")]
        ZIP = 14,
        /// <summary>
        /// RAR archive format
        /// </summary>
        [Description("application/x-rar-compressed")]
        RAR = 15,
        /// <summary>
        /// Generic binary file format
        /// </summary>
        [Description("application/octet-stream")]
        BINARY = 16,

        // Web formats
        /// <summary>
        /// HyperText Markup Language format
        /// </summary>
        [Description("text/html")]
        HTML = 17,
        /// <summary>
        /// URL-encoded form data format
        /// </summary>
        [Description("application/x-www-form-urlencoded")]
        FORM_URL_ENCODED = 18,
        /// <summary>
        /// JavaScript source code format
        /// </summary>
        [Description("application/javascript")]
        JAVASCRIPT = 19,
        /// <summary>
        /// Adobe Flash multimedia format
        /// </summary>
        [Description("application/x-shockwave-flash")]
        FLASH = 20,

        // Font formats
        /// <summary>
        /// TrueType Font format
        /// </summary>
        [Description("application/x-font-ttf")]
        TTF = 21,
        /// <summary>
        /// OpenType Font format
        /// </summary>
        [Description("application/x-font-opentype")]
        OTF = 22,
        /// <summary>
        /// Web Open Font Format version 1
        /// </summary>
        [Description("application/x-font-woff")]
        WOFF = 23,
        /// <summary>
        /// Web Open Font Format version 2
        /// </summary>
        [Description("application/x-font-woff2")]
        WOFF2 = 24,
        /// <summary>
        /// Embedded OpenType font format
        /// </summary>
        [Description("application/vnd.ms-fontobject")]
        EOT = 25,

        // OpenDocument formats
        /// <summary>
        /// OpenDocument Text format
        /// </summary>
        [Description("application/vnd.oasis.opendocument.text")]
        ODT = 26,
        /// <summary>
        /// OpenDocument Spreadsheet format
        /// </summary>
        [Description("application/vnd.oasis.opendocument.spreadsheet")]
        ODS = 27,
        /// <summary>
        /// OpenDocument Presentation format
        /// </summary>
        [Description("application/vnd.oasis.opendocument.presentation")]
        ODP = 28,
        /// <summary>
        /// OpenDocument Graphics format
        /// </summary>
        [Description("application/vnd.oasis.opendocument.graphics")]
        ODG = 29,
        /// <summary>
        /// OpenDocument Formula format
        /// </summary>
        [Description("application/vnd.oasis.opendocument.formula")]
        ODF = 30,

        // Legacy Microsoft Office formats
        /// <summary>
        /// Microsoft Excel 97-2003 spreadsheet format
        /// </summary>
        [Description("application/vnd.ms-excel")]
        XLS = 31,
        /// <summary>
        /// Microsoft Word 97-2003 document format
        /// </summary>
        [Description("application/vnd.ms-word")]
        DOC = 32,
        /// <summary>
        /// Microsoft PowerPoint 97-2003 presentation format
        /// </summary>
        [Description("application/vnd.ms-powerpoint")]
        PPT = 33,

        // Additional archive formats
        /// <summary>
        /// 7-Zip archive format
        /// </summary>
        [Description("application/x-7z-compressed")]
        SEVEN_ZIP = 34,
        /// <summary>
        /// Tape Archive format
        /// </summary>
        [Description("application/x-tar")]
        TAR = 35,
        /// <summary>
        /// RAR archive format (older specification)
        /// </summary>
        [Description("application/x-rar")]
        RAR_OLD = 36,
        /// <summary>
        /// Copy In, Copy Out archive format
        /// </summary>
        [Description("application/x-cpio")]
        CPIO = 37,
        /// <summary>
        /// Bzip compression format
        /// </summary>
        [Description("application/x-bzip")]
        BZIP = 38,
        /// <summary>
        /// Bzip2 compression format
        /// </summary>
        [Description("application/x-bzip2")]
        BZIP2 = 39,
        /// <summary>
        /// LZIP compression format
        /// </summary>
        [Description("application/x-lzip")]
        LZIP = 40,
        /// <summary>
        /// LZMA compression format
        /// </summary>
        [Description("application/x-lzma")]
        LZMA = 41,
        /// <summary>
        /// XZ compression format
        /// </summary>
        [Description("application/x-xz")]
        XZ = 42,
        /// <summary>
        /// Zstandard compression format
        /// </summary>
        [Description("application/x-zstd")]
        ZSTD = 43,

        // Disk image and executable formats
        /// <summary>
        /// ISO 9660 disk image format
        /// </summary>
        [Description("application/x-iso9660-image")]
        ISO = 44,
        /// <summary>
        /// Windows executable file format
        /// </summary>
        [Description("application/x-msdownload")]
        EXE = 45,
        /// <summary>
        /// Shell script file format
        /// </summary>
        [Description("application/x-sh")]
        SHELL_SCRIPT = 46,
        /// <summary>
        /// MS-DOS program file format
        /// </summary>
        [Description("application/x-msdos-program")]
        DOS_PROGRAM = 47,
        /// <summary>
        /// Java Archive file format
        /// </summary>
        [Description("application/x-java-archive")]
        JAR = 48,

        // Apple-specific formats
        /// <summary>
        /// Apple Disk Image format
        /// </summary>
        [Description("application/x-apple-diskimage")]
        DMG = 49,
        /// <summary>
        /// macOS application bundle format
        /// </summary>
        [Description("application/x-apple-diskimage")]
        APP = 50,
        /// <summary>
        /// macOS installer package format
        /// </summary>
        [Description("application/x-apple-diskimage")]
        PKG = 51,
        /// <summary>
        /// iOS application archive format
        /// </summary>
        [Description("application/x-apple-diskimage")]
        IPA = 52,
        /// <summary>
        /// Debian software package format
        /// </summary>
        [Description("application/x-apple-diskimage")]
        DEB = 53,
        /// <summary>
        /// JPEG image format
        /// </summary>
        [Description("image/jpeg")]
        JPG = 54,
        /// <summary>
        /// WebP image format
        /// </summary>
        [Description("image/webp")]
        WEBP = 55,
        // Default/unknown format
        /// <summary>
        /// Unknown or unrecognized file format
        /// </summary>
        [Description("unknown")]
        UNKNOWN = 100
    }
}