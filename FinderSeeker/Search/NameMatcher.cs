using System.IO.Enumeration;
using System.Text.RegularExpressions;

namespace FinderSeeker.Search
{
    /// <summary>
    /// Decides whether a file or folder name matches a pattern, without allocating (it works on spans).
    /// </summary>
    public sealed class NameMatcher
    {
        private readonly string[] _terms;
        private readonly Regex? _regex;
        private readonly MatchMode _mode;
        private readonly bool _ignoreCase;
        private readonly StringComparison _comparison;

        private NameMatcher(MatchMode mode, string[] terms, Regex? regex, bool ignoreCase)
        {
            _mode = mode;
            _terms = terms;
            _regex = regex;
            _ignoreCase = ignoreCase;
            _comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        }

        /// <summary>
        /// Creates a matcher, or null when the pattern is empty (which matches everything).
        /// </summary>
        /// <param name="wrapPlainWildcards">A wildcard pattern with no wildcard in it ("report") means "contains" ("*report*").</param>
        /// <exception cref="ArgumentException">The regular expression is not valid.</exception>
        public static NameMatcher? Create(string? pattern, MatchMode mode, bool caseSensitive, bool wrapPlainWildcards = true)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                return null;
            }

            if (mode == MatchMode.Regex)
            {
                var options = RegexOptions.CultureInvariant | RegexOptions.Compiled;
                if (!caseSensitive)
                {
                    options |= RegexOptions.IgnoreCase;
                }
                return new NameMatcher(mode, [], new Regex(pattern, options, TimeSpan.FromSeconds(2)), !caseSensitive);
            }

            var terms = pattern.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (terms.Length == 0)
            {
                return null;
            }

            if (mode == MatchMode.Wildcard)
            {
                for (int i = 0; i < terms.Length; i++)
                {
                    if (terms[i] == "*.*")
                    {
                        terms[i] = "*";
                    }
                    else if (wrapPlainWildcards && terms[i].AsSpan().IndexOfAny('*', '?') < 0)
                    {
                        terms[i] = "*" + terms[i] + "*";
                    }
                }
            }

            return new NameMatcher(mode, terms, null, !caseSensitive);
        }

        public bool IsMatch(ReadOnlySpan<char> name)
        {
            switch (_mode)
            {
                case MatchMode.Regex:
                    try
                    {
                        return _regex!.IsMatch(name);
                    }
                    catch (RegexMatchTimeoutException)
                    {
                        return false;
                    }

                case MatchMode.PlainText:
                    foreach (var term in _terms)
                    {
                        if (name.Contains(term, _comparison))
                        {
                            return true;
                        }
                    }
                    return false;

                default:
                    foreach (var term in _terms)
                    {
                        if (FileSystemName.MatchesSimpleExpression(term, name, _ignoreCase))
                        {
                            return true;
                        }
                    }
                    return false;
            }
        }
    }
}
