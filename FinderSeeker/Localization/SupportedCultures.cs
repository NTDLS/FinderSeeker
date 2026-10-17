using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace FinderSeeker.Localization
{
    public class CultureInfoSettings
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public CultureInfo Culture { get; set; }

        public CultureInfoSettings(string code, string name)
        {
            Code = code;
            Name = name;
            Culture = new CultureInfo(code);
        }
    }

    public class SupportedCultures
    {
        public IList<CultureInfoSettings> Collection { get; private set; }

        public bool TryGetByCode(string code, [NotNullWhen(true)] out CultureInfoSettings? result)
        {
            result = Collection.FirstOrDefault(o => o.Code == code);
            return result != null;
        }

        public SupportedCultures()
        {
            Collection = new List<CultureInfoSettings>
            {
                new("en", "English"), new("ar", "Arabic"), new("az", "Azerbaijani"), new("be", "Belarusian"),
                new("bg", "Bulgarian"), new("cs", "Czech"), new("da", "Danish"), new("de", "German"),
                new("el", "Greek"), new("es", "Spanish"), new("et", "Estonian"), new("fa", "Persian"),
                new("fi", "Finnish"), new("fr", "French"), new("he", "Hebrew"), new("hr", "Croatian"),
                new("hu", "Hungarian"), new("id", "Indonesian"), new("is", "Icelandic"), new("it", "Italian"),
                new("ja", "Japanese"), new("ka", "Georgian"), new("kk", "Kazakh"), new("ko", "Korean"),
                new("lt", "Lithuanian"), new("lv", "Latvian"), new("ms", "Malay"), new("nl", "Dutch"),
                new("nn", "Norwegian"), new("nb", "Bokmål"), new("pl", "Polish"), new("pt", "Portuguese"),
                new("ro", "Romanian"), new("ru", "Russian"), new("sk", "Slovak"), new("sl", "Slovenian"),
                new("sr", "Serbian"), new("sv", "Swedish"), new("th", "Thai"), new("tr", "Turkish"),
                new("uk", "Ukrainian"), new("vi", "Vietnamese"), new("zh-Hans", "Chinese simplified"),
                new("zh-Hant", "Chinese traditional"), new("bn", "Bengali"), new("hi", "Hindi"), new("ur", "Urdu")
            };
        }
    }
}
