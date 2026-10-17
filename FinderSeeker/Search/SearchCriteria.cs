namespace FinderSeeker.Search
{
    /// <summary>
    /// How a text is interpreted.
    /// </summary>
    public enum MatchMode
    {
        /// <summary>Wildcards: * is any text, ? is any one character.</summary>
        Wildcard,
        /// <summary>The text as typed.</summary>
        PlainText,
        /// <summary>A .NET regular expression.</summary>
        Regex
    }

    public enum ItemKinds
    {
        Files,
        Folders,
        FilesAndFolders
    }

    /// <summary>
    /// Everything that decides which files are found.
    /// </summary>
    public class SearchCriteria
    {
        /// <summary>The folders (or drives) to search.</summary>
        public List<string> Locations { get; set; } = new();
        public bool IncludeSubfolders { get; set; } = true;

        /// <summary>What to look at: files, folders or both. Filters that only make sense for files (type, size, content) limit it to files.</summary>
        public ItemKinds Kinds { get; set; } = ItemKinds.Files;

        public string NamePattern { get; set; } = string.Empty;
        public MatchMode NameMode { get; set; } = MatchMode.Wildcard;
        public bool NameCaseSensitive { get; set; }

        /// <summary>Match the name pattern against the whole path rather than just the name.</summary>
        public bool NameMatchesFullPath { get; set; }

        /// <summary>Wildcard names (files and folders) to leave out, separated by semicolons: "node_modules;.git;*.tmp". Excluded folders are not entered.</summary>
        public string Exclude { get; set; } = string.Empty;

        public FileCategory Category { get; set; } = FileCategory.Any;

        public long? MinSize { get; set; }
        public long? MaxSize { get; set; }

        public DateTime? ModifiedAfter { get; set; }
        public DateTime? ModifiedBefore { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }

        public bool IncludeHidden { get; set; } = true;
        public bool IncludeSystem { get; set; }

        /// <summary>Text to find inside files. Empty means the contents are not searched.</summary>
        public string ContentPattern { get; set; } = string.Empty;
        public MatchMode ContentMode { get; set; } = MatchMode.PlainText;
        public bool ContentCaseSensitive { get; set; }

        /// <summary>Also look inside files that do not look like text.</summary>
        public bool SearchBinaryFiles { get; set; }

        /// <summary>Files larger than this are not searched for content. Zero means no limit.</summary>
        public long MaxContentFileSize { get; set; }

        public bool HasContentSearch => !string.IsNullOrEmpty(ContentPattern);

        /// <summary>
        /// True when only files can be results: folders have no type, size or contents.
        /// </summary>
        public bool FilesOnly => Kinds == ItemKinds.Files
            || Category != FileCategory.Any || MinSize != null || MaxSize != null || HasContentSearch;
    }

    /// <summary>
    /// A kind of file, as a set of extensions.
    /// </summary>
    public enum FileCategory
    {
        Any,
        Text,
        SourceCode,
        Images,
        Documents,
        Audio,
        Video,
        Archives,
        Executables
    }

    public static class FileCategories
    {
        private static readonly Dictionary<FileCategory, string[]> _extensions = new()
        {
            [FileCategory.Text] = [".txt", ".log", ".md", ".markdown", ".csv", ".tsv", ".ini", ".cfg", ".conf", ".config", ".json", ".xml", ".yaml", ".yml", ".toml", ".properties", ".rtf", ".tex", ".srt", ".nfo"],
            [FileCategory.SourceCode] = [".cs", ".vb", ".fs", ".c", ".h", ".cpp", ".hpp", ".cc", ".cxx", ".js", ".mjs", ".ts", ".tsx", ".jsx", ".py", ".java", ".kt", ".go", ".rs", ".php", ".rb", ".swift", ".sql", ".ps1", ".psm1", ".bat", ".cmd", ".sh", ".html", ".htm", ".css", ".scss", ".less", ".xaml", ".razor", ".cshtml", ".csproj", ".vbproj", ".sln", ".slnx", ".props", ".targets", ".resx", ".iss", ".asm", ".lua", ".pl", ".r", ".dart"],
            [FileCategory.Images] = [".bmp", ".png", ".jpg", ".jpeg", ".jpe", ".gif", ".tif", ".tiff", ".ico", ".webp", ".svg", ".heic", ".emf", ".wmf", ".psd", ".raw"],
            [FileCategory.Documents] = [".doc", ".docx", ".dot", ".dotx", ".xls", ".xlsx", ".xlsm", ".ppt", ".pptx", ".pdf", ".odt", ".ods", ".odp", ".rtf", ".vsd", ".vsdx", ".pub", ".one"],
            [FileCategory.Audio] = [".mp3", ".wav", ".flac", ".aac", ".ogg", ".wma", ".m4a", ".opus", ".aiff", ".mid", ".midi"],
            [FileCategory.Video] = [".mp4", ".mkv", ".avi", ".mov", ".wmv", ".flv", ".webm", ".m4v", ".mpg", ".mpeg", ".3gp", ".ts"],
            [FileCategory.Archives] = [".zip", ".7z", ".rar", ".tar", ".gz", ".tgz", ".bz2", ".xz", ".cab", ".iso", ".wim", ".zst"],
            [FileCategory.Executables] = [".exe", ".dll", ".msi", ".sys", ".com", ".scr", ".ocx", ".drv", ".cpl", ".msix", ".appx"]
        };

        /// <summary>
        /// The extensions (with their dot) of a category, or an empty array for <see cref="FileCategory.Any"/>.
        /// </summary>
        public static string[] ExtensionsOf(FileCategory category)
            => _extensions.TryGetValue(category, out var extensions) ? extensions : [];

        /// <summary>
        /// The text shown for a category in the type list.
        /// </summary>
        public static string DisplayName(FileCategory category) => category switch
        {
            FileCategory.Any => Localize("Any type"),
            FileCategory.Text => Localize("Text files"),
            FileCategory.SourceCode => Localize("Source code"),
            FileCategory.Images => Localize("Images"),
            FileCategory.Documents => Localize("Documents"),
            FileCategory.Audio => Localize("Audio"),
            FileCategory.Video => Localize("Video"),
            FileCategory.Archives => Localize("Archives"),
            FileCategory.Executables => Localize("Programs"),
            _ => category.ToString()
        };
    }
}
