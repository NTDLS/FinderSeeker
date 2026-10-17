namespace FinderSeeker.Localization
{
    /// <summary>
    /// Decides what counts as wording that should be translated. Shared with the LocalizerScan tool.
    /// </summary>
    public static class Phrases
    {
        /// <summary>
        /// True if text has at least one letter and is not a web address. Numbers, symbols and addresses are the same in every language.
        /// </summary>
        public static bool IsTranslatable(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            var trimmed = text.Trim();
            if (trimmed.StartsWith("www.", StringComparison.OrdinalIgnoreCase) || trimmed.Contains("://"))
            {
                return false;
            }

            return trimmed.Any(char.IsLetter);
        }
    }
}
