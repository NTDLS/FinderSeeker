using FinderSeeker.Search;
using Newtonsoft.Json;

namespace FinderSeeker
{
    /// <summary>
    /// What is remembered between runs, in %AppData%\NTDLS\FinderSeeker\settings.json. Nothing is stored in the registry.
    /// </summary>
    public class AppSettings
    {
        public static string Folder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NTDLS", "FinderSeeker");
        private static readonly JsonSerializerSettings JsonOptions = new()
        {
            Formatting = Formatting.Indented,
            Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };

        public static string FilePath => Path.Combine(Folder, "settings.json");

        /// <summary>A culture name such as "de" or "pt-BR". Empty follows the Windows language.</summary>
        public string Language { get; set; } = string.Empty;

        public List<string> LocationHistory { get; set; } = new();
        public List<string> NameHistory { get; set; } = new();
        public List<string> ContentHistory { get; set; } = new();

        public string Location { get; set; } = string.Empty;
        public bool IncludeSubfolders { get; set; } = true;
        public string Name { get; set; } = string.Empty;
        public MatchMode NameMode { get; set; } = MatchMode.Wildcard;
        public bool NameCaseSensitive { get; set; }

        public bool UseMinSize { get; set; }
        public decimal MinSize { get; set; }
        public int MinSizeUnit { get; set; } = 1;
        public bool UseMaxSize { get; set; }
        public decimal MaxSize { get; set; }
        public int MaxSizeUnit { get; set; } = 1;

        public DateTime? ModifiedAfter { get; set; }
        public DateTime? ModifiedBefore { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }

        public bool IncludeHidden { get; set; } = true;
        public bool IncludeSystem { get; set; }

        public string Content { get; set; } = string.Empty;
        public MatchMode ContentMode { get; set; } = MatchMode.PlainText;
        public bool ContentCaseSensitive { get; set; }
        public bool SearchBinaryFiles { get; set; }
        public decimal MaxContentFileSizeMb { get; set; } = 256;

        public bool ShowPreview { get; set; } = true;
        public int PreviewWidth { get; set; } = 420;
        public Rectangle? WindowBounds { get; set; }
        public bool WindowMaximized { get; set; }

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(FilePath), JsonOptions) ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                Log.Error("Unable to read the settings. Defaults will be used.", ex);
            }

            return new AppSettings();
        }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Folder);
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(this, JsonOptions));
            }
            catch (Exception ex)
            {
                Log.Error("Unable to save the settings.", ex);
            }
        }
    }
}
