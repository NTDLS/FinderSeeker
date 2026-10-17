using System.Buffers;
using System.Text;
using System.Text.RegularExpressions;

namespace FinderSeeker.Search
{
    public enum ContentOutcome
    {
        NoMatch,
        Match,
        /// <summary>The file looks binary and binary files are not being searched.</summary>
        SkippedBinary,
        /// <summary>The file could not be read (locked, no access, a pattern that took too long).</summary>
        Error
    }

    /// <summary>
    /// Finds text inside files. Plain text is found with vectorized span searches over large blocks of the file, so it is
    /// limited by the disk. Wildcard and regular expression patterns are matched one line at a time, still without allocating
    /// a string per line.
    ///
    /// A match never spans lines for wildcard and regex patterns (so "." and "\s" stay within the line).
    /// Plain text is found across lines too, so it may contain a line break.
    /// </summary>
    public sealed class ContentMatcher
    {
        private const int BlockChars = 128 * 1024;
        private const int MaxLineChars = 16 * 1024 * 1024;
        private const int MaxPlainTextLength = 32 * 1024;
        private const int SnippetRadius = 80;

        private readonly string? _needle;
        private readonly StringComparison _comparison;
        private readonly Regex? _regex;

        private ContentMatcher(string? needle, StringComparison comparison, Regex? regex)
        {
            _needle = needle;
            _comparison = comparison;
            _regex = regex;
        }

        /// <summary>
        /// True when matching is done line by line (wildcard and regex), false for plain text.
        /// </summary>
        public bool IsLineBased => _regex != null;

        /// <exception cref="ArgumentException">The pattern is not valid.</exception>
        public static ContentMatcher Create(string pattern, MatchMode mode, bool caseSensitive)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException("The text to find is empty.", nameof(pattern));
            }

            if (mode == MatchMode.PlainText)
            {
                if (pattern.Length > MaxPlainTextLength)
                {
                    throw new ArgumentException(Localize("The text to find is too long."), nameof(pattern));
                }
                return new ContentMatcher(pattern, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase, null);
            }

            var expression = mode == MatchMode.Wildcard ? WildcardToRegex(pattern) : pattern;

            //Multiline makes ^ and $ mean the start and end of a line when text is matched as a whole (the preview).
            var options = RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.Multiline;
            if (!caseSensitive)
            {
                options |= RegexOptions.IgnoreCase;
            }

            //The timeout stops a pattern that backtracks without end from holding up a search.
            return new ContentMatcher(null, StringComparison.Ordinal, new Regex(expression, options, TimeSpan.FromSeconds(2)));
        }

        /// <summary>
        /// "*" is any run of characters and "?" is any one character, found anywhere in a line.
        /// </summary>
        public static string WildcardToRegex(string pattern)
        {
            var text = new StringBuilder(pattern.Length * 2);
            foreach (var c in pattern)
            {
                switch (c)
                {
                    case '*': text.Append(".*?"); break;
                    case '?': text.Append('.'); break;
                    default: text.Append(Regex.Escape(c.ToString())); break;
                }
            }
            return text.ToString();
        }

        /// <summary>
        /// Finds the first match in some text.
        /// </summary>
        public bool TryFind(ReadOnlySpan<char> text, out int index, out int length)
        {
            if (_regex == null)
            {
                index = text.IndexOf(_needle, _comparison);
                length = _needle!.Length;
                return index >= 0;
            }

            foreach (var match in _regex.EnumerateMatches(text))
            {
                if (match.Length > 0)
                {
                    index = match.Index;
                    length = match.Length;
                    return true;
                }
            }

            index = length = 0;
            return false;
        }

        /// <summary>
        /// Every match in some text (up to a limit), for highlighting.
        /// </summary>
        public List<(int Index, int Length)> FindAll(string text, int limit)
        {
            var found = new List<(int, int)>();

            try
            {
                if (_regex == null)
                {
                    for (int at = 0; found.Count < limit && at < text.Length;)
                    {
                        int index = text.AsSpan(at).IndexOf(_needle, _comparison);
                        if (index < 0)
                        {
                            break;
                        }
                        found.Add((at + index, _needle!.Length));
                        at += index + _needle.Length;
                    }
                }
                else
                {
                    foreach (var match in _regex.EnumerateMatches(text))
                    {
                        if (match.Length > 0)
                        {
                            found.Add((match.Index, match.Length));
                            if (found.Count >= limit)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (RegexMatchTimeoutException)
            {
                //Highlight what was found.
            }

            return found;
        }

        /// <summary>
        /// Looks inside one file for the first match.
        /// </summary>
        /// <param name="line">The line the match is on (starting at 1).</param>
        /// <param name="snippet">The text around the match.</param>
        public ContentOutcome SearchFile(string path, bool searchBinaryFiles, CancellationToken cancellationToken, out int line, out string snippet)
        {
            line = 0;
            snippet = string.Empty;

            try
            {
                using var stream = new FileStream(path, new FileStreamOptions
                {
                    Mode = FileMode.Open,
                    Access = FileAccess.Read,
                    Share = FileShare.ReadWrite | FileShare.Delete,
                    Options = FileOptions.SequentialScan,
                    BufferSize = 0
                });

                //Look at the start of the file to find its encoding, and whether it is text at all.
                var head = new byte[4096];
                int headLength = 0;
                int read;
                while (headLength < head.Length && (read = stream.Read(head, headLength, head.Length - headLength)) > 0)
                {
                    headLength += read;
                }

                bool hasByteOrderMark = HasByteOrderMark(head.AsSpan(0, headLength));
                bool binary = !hasByteOrderMark && head.AsSpan(0, headLength).Contains((byte)0);

                if (binary && !searchBinaryFiles)
                {
                    return ContentOutcome.SkippedBinary;
                }

                stream.Position = 0;
                using var reader = new StreamReader(stream, binary ? Encoding.Latin1 : new UTF8Encoding(false),
                    detectEncodingFromByteOrderMarks: hasByteOrderMark, bufferSize: 64 * 1024, leaveOpen: true);

                var found = _regex == null
                    ? SearchBlocks(reader, cancellationToken, out line, out snippet)
                    : SearchLines(reader, cancellationToken, out line, out snippet);

                return found ? ContentOutcome.Match : ContentOutcome.NoMatch;
            }
            catch (RegexMatchTimeoutException)
            {
                return ContentOutcome.Error;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or NotSupportedException or ArgumentException)
            {
                return ContentOutcome.Error;
            }
        }

        private static bool HasByteOrderMark(ReadOnlySpan<byte> head)
        {
            return head.Length >= 3 && head[0] == 0xEF && head[1] == 0xBB && head[2] == 0xBF
                || head.Length >= 2 && head[0] == 0xFF && head[1] == 0xFE
                || head.Length >= 2 && head[0] == 0xFE && head[1] == 0xFF
                || head.Length >= 4 && head[0] == 0 && head[1] == 0 && head[2] == 0xFE && head[3] == 0xFF;
        }

        /// <summary>
        /// Plain text: search whole blocks, keeping the end of each so a match across two blocks is found.
        /// </summary>
        private bool SearchBlocks(StreamReader reader, CancellationToken cancellationToken, out int line, out string snippet)
        {
            line = 0;
            snippet = string.Empty;

            var buffer = ArrayPool<char>.Shared.Rent(BlockChars);
            try
            {
                int filled = 0;
                int linesBefore = 0; //Line breaks in the text already dropped from the front of the buffer.
                int keep = _needle!.Length - 1;

                while (!cancellationToken.IsCancellationRequested)
                {
                    int read = reader.Read(buffer, filled, buffer.Length - filled);
                    if (read <= 0)
                    {
                        break;
                    }
                    filled += read;

                    var text = buffer.AsSpan(0, filled);
                    int index = text.IndexOf(_needle, _comparison);
                    if (index >= 0)
                    {
                        line = linesBefore + text[..index].Count('\n') + 1;
                        snippet = MakeSnippet(text, index, _needle.Length);
                        return true;
                    }

                    int retained = Math.Min(filled, keep);
                    int dropped = filled - retained;
                    linesBefore += text[..dropped].Count('\n');
                    text[dropped..].CopyTo(buffer);
                    filled = retained;
                }

                return false;
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buffer);
            }
        }

        /// <summary>
        /// Wildcard and regex: match one line at a time, straight from the buffer.
        /// </summary>
        private bool SearchLines(StreamReader reader, CancellationToken cancellationToken, out int line, out string snippet)
        {
            line = 0;
            snippet = string.Empty;

            var buffer = ArrayPool<char>.Shared.Rent(BlockChars);
            try
            {
                int filled = 0;
                int lineNumber = 0;

                while (!cancellationToken.IsCancellationRequested)
                {
                    int read = reader.Read(buffer, filled, buffer.Length - filled);
                    bool endOfFile = read <= 0;
                    if (!endOfFile)
                    {
                        filled += read;
                    }

                    int start = 0;
                    while (true)
                    {
                        var rest = buffer.AsSpan(start, filled - start);
                        int newLine = rest.IndexOf('\n');
                        if (newLine < 0)
                        {
                            break;
                        }

                        lineNumber++;
                        if (TryMatchLine(rest[..newLine], lineNumber, out line, out snippet))
                        {
                            return true;
                        }
                        start += newLine + 1;
                    }

                    if (endOfFile)
                    {
                        if (start < filled && TryMatchLine(buffer.AsSpan(start, filled - start), lineNumber + 1, out line, out snippet))
                        {
                            return true;
                        }
                        return false;
                    }

                    int remaining = filled - start;
                    if (remaining == buffer.Length)
                    {
                        //One line fills the whole buffer.
                        if (buffer.Length < MaxLineChars)
                        {
                            var larger = ArrayPool<char>.Shared.Rent(buffer.Length * 2);
                            buffer.AsSpan(0, remaining).CopyTo(larger);
                            ArrayPool<char>.Shared.Return(buffer);
                            buffer = larger;
                        }
                        else
                        {
                            //Absurdly long line: match the part we have and move on.
                            if (TryMatchLine(buffer.AsSpan(0, remaining), lineNumber + 1, out line, out snippet))
                            {
                                return true;
                            }
                            remaining = 0;
                        }
                    }
                    else if (start > 0)
                    {
                        buffer.AsSpan(start, remaining).CopyTo(buffer);
                    }
                    filled = remaining;
                }

                return false;
            }
            finally
            {
                ArrayPool<char>.Shared.Return(buffer);
            }
        }

        private bool TryMatchLine(ReadOnlySpan<char> text, int lineNumber, out int line, out string snippet)
        {
            if (text.Length > 0 && text[^1] == '\r')
            {
                text = text[..^1];
            }

            if (TryFind(text, out var index, out var length))
            {
                line = lineNumber;
                snippet = MakeSnippet(text, index, length);
                return true;
            }

            line = 0;
            snippet = string.Empty;
            return false;
        }

        /// <summary>
        /// The line a match is on, cut down to a readable length around the match.
        /// </summary>
        private static string MakeSnippet(ReadOnlySpan<char> text, int index, int length)
        {
            int start = text[..index].LastIndexOf('\n') + 1;
            int endOffset = text[(index + length)..].IndexOf('\n');
            int end = endOffset < 0 ? text.Length : index + length + endOffset;

            start = Math.Max(start, index - SnippetRadius);
            end = Math.Min(end, index + length + SnippetRadius);

            var snippet = text[start..end].ToString().Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ').Trim();
            return (start > 0 && text[start - 1] != '\n' ? "…" : "") + snippet;
        }
    }
}
