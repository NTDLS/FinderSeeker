using System.Collections.Concurrent;
using System.Globalization;

namespace FinderSeeker.Localization
{
    /// <summary>
    /// Looks up display text in the translation database. The English text in the source code is the key:
    /// <c>Localize("Search")</c> returns the translation of "Search" for the current UI language, and returns "Search" itself
    /// when there is no translation, so a missing database only ever costs the translation, never the text.
    /// </summary>
    public class Localizer
    {
        private static Localizer? _default;

        public static string DefaultDatabaseFile => Path.Combine(AppContext.BaseDirectory, "Translations", "Translations.db");

        public static Localizer Default
        {
            get => _default ??= new Localizer(DefaultDatabaseFile);
            set => _default = value;
        }

        private readonly TranslationDatabase _database;
        private readonly ConcurrentDictionary<string, bool> _reported = new();

        public Localizer(string databaseFile)
        {
            _database = new TranslationDatabase(databaseFile);
        }

        /// <summary>
        /// The language to translate to. When null the current UI culture (the Windows display language) is used.
        /// </summary>
        public CultureInfo? Culture { get; set; }

        /// <summary>
        /// Chooses the language: a culture name such as "de" or "pt-BR", or null, empty or "auto" to follow Windows.
        /// </summary>
        public void SetLanguage(string? cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName) || cultureName.Trim().Equals("auto", StringComparison.OrdinalIgnoreCase))
            {
                Culture = null;
                return;
            }

            try
            {
                Culture = CultureInfo.GetCultureInfo(cultureName.Trim(), predefinedOnly: true);
            }
            catch (CultureNotFoundException ex)
            {
                Log.Warning($"\"{cultureName}\" is not a language that can be used. The Windows language will be used.", ex);
                Culture = null;
            }
        }

        private CultureInfo EffectiveCulture => Culture ?? CultureInfo.CurrentUICulture;

        public string Localize(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return key;
            }

            return _database.TryGetTranslation(EffectiveCulture, key, out var translation) ? translation : key;
        }

        public string Localize(string key, params object[] args)
        {
            if (string.IsNullOrEmpty(key))
            {
                return key;
            }

            var text = Localize(key);
            try
            {
                return string.Format(text, args);
            }
            catch (FormatException ex)
            {
                if (_reported.TryAdd(key, true))
                {
                    Log.Warning($"The translation for \"{key}\" cannot be formatted, so the original text is used.", ex);
                }

                try
                {
                    return string.Format(key, args);
                }
                catch (FormatException)
                {
                    return key;
                }
            }
        }
    }
}
