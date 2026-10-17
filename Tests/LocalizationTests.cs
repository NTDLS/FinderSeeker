using FinderSeeker.Localization;
using System.Globalization;
using Xunit;

namespace Tests
{
    public class LocalizationTests
    {
        private static Localizer Create(string culture) => new(Localizer.DefaultDatabaseFile) { Culture = new CultureInfo(culture) };

        [Fact]
        public void TranslationDatabaseIsDeployed() => Assert.True(File.Exists(Localizer.DefaultDatabaseFile));

        [Fact]
        public void TranslatesToTheChosenLanguage()
        {
            Assert.NotEqual("Cancel", Create("de").Localize("Cancel"));
            Assert.Equal("Cancel", Create("en").Localize("Cancel"));
        }

        [Fact]
        public void MissingTranslationFallsBackToTheEnglishText()
            => Assert.Equal("Not in the database at all", Create("de").Localize("Not in the database at all"));

        [Fact]
        public void RegionalCultureUsesItsLanguage()
            => Assert.Equal(Create("de").Localize("Cancel"), Create("de-AT").Localize("Cancel"));

        [Fact]
        public void FormatsArguments()
            => Assert.Equal("5 found", Create("en").Localize("{0} found", 5));

        [Fact]
        public void BrokenTranslationPlaceholderFallsBack()
            => Assert.Equal("a b", new Localizer("missing.db").Localize("{0} {1}", "a", "b"));

        [Fact]
        public void EveryCultureLocalizesWithoutThrowing()
        {
            foreach (var culture in new SupportedCultures().Collection)
            {
                Assert.False(string.IsNullOrEmpty(Create(culture.Code).Localize("Search")));
            }
        }
    }
}
