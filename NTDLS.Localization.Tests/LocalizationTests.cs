using Microsoft.Data.Sqlite;
using NTDLS.Localization;
using System.Globalization;
using Xunit;

namespace NTDLS.Localization.Tests
{
    public sealed class LocalizationTests : IDisposable
    {
        private readonly string _folder = Directory.CreateTempSubdirectory("ntdls-localization-").FullName;
        private readonly string _database;

        public LocalizationTests()
        {
            _database = Path.Combine(_folder, "Translations.db");

            using var connection = new SqliteConnection($"Data Source={_database};Pooling=False");
            connection.Open();

            void Run(string sql)
            {
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }

            //Columns are named by culture code. The key column is not called "Id", because "id" is the code for Indonesian.
            Run("""CREATE TABLE "Translation" ("TranslationId" INTEGER PRIMARY KEY AUTOINCREMENT, "Phrase" TEXT NOT NULL UNIQUE COLLATE NOCASE, "de" TEXT, "fr" TEXT, "nb" TEXT, "id" TEXT)""");
            Run("""INSERT INTO "Translation" (Phrase, de, fr, nb, id) VALUES ('Cancel', 'Abbrechen', 'Annuler', 'Avbryt', 'Batal')""");
            Run("""INSERT INTO "Translation" (Phrase, de) VALUES ('Found {0} files', '{0} Dateien gefunden')""");
            Run("""INSERT INTO "Translation" (Phrase, de) VALUES ('Broken {0}', '{0} {1}')""");
            Run("""INSERT INTO "Translation" (Phrase, de) VALUES ('Untranslated', '')""");
        }

        public void Dispose()
        {
            SqliteConnection.ClearAllPools();
            Directory.Delete(_folder, true);
        }

        private Localizer Create(string culture) => new(_database) { Culture = new CultureInfo(culture) };

        [Fact]
        public void TranslatesToTheChosenLanguage()
        {
            Assert.Equal("Abbrechen", Create("de").Localize("Cancel"));
            Assert.Equal("Annuler", Create("fr").Localize("Cancel"));
        }

        [Fact]
        public void IndonesianIsNotMistakenForTheKeyColumn() => Assert.Equal("Batal", Create("id").Localize("Cancel"));

        [Fact]
        public void DatabaseWithTheOlderLanguageNamesStillWorks()
        {
            var legacy = Path.Combine(_folder, "legacy.db");
            using (var connection = new SqliteConnection($"Data Source={legacy};Pooling=False"))
            {
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = """
                    CREATE TABLE "Translation" ("Id" INTEGER PRIMARY KEY AUTOINCREMENT, "English" TEXT NOT NULL UNIQUE COLLATE NOCASE, "German" TEXT, "Chinese simplified" TEXT, "Bokmål" TEXT);
                    INSERT INTO "Translation" (English, German, "Chinese simplified", "Bokmål") VALUES ('Cancel', 'Abbrechen', '取消', 'Avbryt');
                    """;
                command.ExecuteNonQuery();
            }

            Assert.Equal("Abbrechen", new Localizer(legacy) { Culture = new CultureInfo("de") }.Localize("Cancel"));
            Assert.Equal("取消", new Localizer(legacy) { Culture = new CultureInfo("zh-CN") }.Localize("Cancel"));
            Assert.Equal("Avbryt", new Localizer(legacy) { Culture = new CultureInfo("nb") }.Localize("Cancel"));
        }

        [Fact]
        public void EnglishNeedsNoTranslation() => Assert.Equal("Cancel", Create("en").Localize("Cancel"));

        [Fact]
        public void KeyIsCaseInsensitive() => Assert.Equal("Abbrechen", Create("de").Localize("CANCEL"));

        [Fact]
        public void MissingOrEmptyTranslationFallsBackToTheEnglishText()
        {
            Assert.Equal("Not in the database", Create("de").Localize("Not in the database"));
            Assert.Equal("Untranslated", Create("de").Localize("Untranslated"));
            Assert.Equal("Cancel", Create("ja").Localize("Cancel"));
        }

        [Fact]
        public void RegionalCultureUsesItsLanguage() => Assert.Equal("Abbrechen", Create("de-AT").Localize("Cancel"));

        [Fact]
        public void PlainNorwegianUsesTheBokmalColumn() => Assert.Equal("Avbryt", Create("nb").Localize("Cancel"));

        [Fact]
        public void FormatsArgumentsIntoTheTranslation() => Assert.Equal("5 Dateien gefunden", Create("de").Localize("Found {0} files", 5));

        [Fact]
        public void BrokenTranslationPlaceholderFallsBackToTheEnglishText()
        {
            var warnings = new List<string>();
            LocalizationDiagnostics.OnWarning = (message, _) => warnings.Add(message);
            try
            {
                Assert.Equal("Broken 7", Create("de").Localize("Broken {0}", 7));
                Assert.Single(warnings);
            }
            finally
            {
                LocalizationDiagnostics.OnWarning = null;
            }
        }

        [Fact]
        public void MissingDatabaseShowsEnglishAndReportsOnce()
        {
            var warnings = new List<string>();
            LocalizationDiagnostics.OnWarning = (message, _) => warnings.Add(message);
            try
            {
                var localizer = new Localizer(Path.Combine(_folder, "missing.db")) { Culture = new CultureInfo("de") };
                Assert.Equal("Cancel", localizer.Localize("Cancel"));
                Assert.Single(warnings);
                Assert.False(File.Exists(Path.Combine(_folder, "missing.db")), "A missing database must not be created.");
            }
            finally
            {
                LocalizationDiagnostics.OnWarning = null;
            }
        }

        [Fact]
        public void SetLanguageChoosesTheCultureAndAutoFollowsWindows()
        {
            var localizer = new Localizer(_database);
            localizer.SetLanguage("de");
            Assert.Equal("Abbrechen", localizer.Localize("Cancel"));

            localizer.SetLanguage("auto");
            Assert.Null(localizer.Culture);
            Assert.Equal(CultureInfo.CurrentUICulture, localizer.EffectiveCulture);

            localizer.SetLanguage("not a language!");
            Assert.Null(localizer.Culture);
        }

        [Fact]
        public void WorksThroughTheInterface()
        {
            ILocalizer localizer = Create("de");
            Assert.Equal("Abbrechen", localizer.Localize("Cancel"));
            Assert.Equal("de", localizer.EffectiveCulture.Name);
        }

        [Fact]
        public void NullLocalizerReturnsTheEnglishText()
        {
            ILocalizer localizer = NullLocalizer.Instance;
            Assert.Equal("Cancel", localizer.Localize("Cancel"));
            Assert.Equal("3 files", localizer.Localize("{0} files", 3));
            Assert.Equal("{broken", localizer.Localize("{broken", 1));
        }

        [Fact]
        public void StaticHelpersUseTheDefaultLocalizer()
        {
            var original = Localizer.Default;
            try
            {
                Localizer.Default = Create("fr");
                Assert.Equal("Annuler", StaticLocalizer.Localize("Cancel"));
            }
            finally
            {
                Localizer.Default = original;
            }
        }

        [Theory]
        [InlineData("Search", true)]
        [InlineData("  ", false)]
        [InlineData("1234", false)]
        [InlineData("https://example.com", false)]
        [InlineData("www.example.com", false)]
        [InlineData("v2", true)]
        public void PhrasesDecidesWhatToTranslate(string text, bool expected) => Assert.Equal(expected, Phrases.IsTranslatable(text));

        [Fact]
        public void EverySupportedCultureLocalizesWithoutThrowing()
        {
            foreach (var culture in new SupportedCultures().Collection)
            {
                Assert.Equal("Not translated anywhere", Create(culture.Code).Localize("Not translated anywhere"));
            }
        }

        [Fact]
        public void FormLocalizerTranslatesDesignerTextAndRemembersTheEnglish()
        {
            using var form = new Form { Text = "Cancel" };
            var button = new Button { Text = "Cancel" };
            var label = new Label { Text = "12345" };
            form.Controls.Add(button);
            form.Controls.Add(label);

            FormLocalizer.Apply(form, Create("de"));
            Assert.Equal("Abbrechen", button.Text);
            Assert.Equal("Abbrechen", form.Text);
            Assert.Equal("12345", label.Text);

            FormLocalizer.Apply(form, Create("fr"));
            Assert.Equal("Annuler", button.Text);
            Assert.Equal("Cancel", FormLocalizer.SourceText(button));

            // Text the code has set itself is left alone.
            button.Text = "Custom";
            FormLocalizer.Apply(form, Create("de"));
            Assert.Equal("Custom", button.Text);
        }
    }
}
