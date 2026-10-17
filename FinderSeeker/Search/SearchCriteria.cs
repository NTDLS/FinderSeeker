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

    /// <summary>
    /// Everything that decides which files are found.
    /// </summary>
    public class SearchCriteria
    {
        /// <summary>The folders (or drives) to search.</summary>
        public List<string> Locations { get; set; } = new();
        public bool IncludeSubfolders { get; set; } = true;

        public string NamePattern { get; set; } = string.Empty;
        public MatchMode NameMode { get; set; } = MatchMode.Wildcard;
        public bool NameCaseSensitive { get; set; }

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
    }
}
