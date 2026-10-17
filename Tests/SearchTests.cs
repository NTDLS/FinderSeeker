using FinderSeeker.Search;
using System.Text;
using Xunit;

namespace Tests
{
    public class NameMatcherTests
    {
        [Theory]
        [InlineData("*.cs", MatchMode.Wildcard, false, "Program.CS", true)]
        [InlineData("*.cs", MatchMode.Wildcard, true, "Program.CS", false)]
        [InlineData("*.cs;*.vb", MatchMode.Wildcard, false, "a.vb", true)]
        [InlineData("report", MatchMode.Wildcard, false, "Annual Report 2024.docx", true)]   // no wildcard means "contains"
        [InlineData("*.*", MatchMode.Wildcard, false, "README", true)]
        [InlineData("a?c.txt", MatchMode.Wildcard, false, "abc.txt", true)]
        [InlineData("a?c.txt", MatchMode.Wildcard, false, "abbc.txt", false)]
        [InlineData("log", MatchMode.PlainText, false, "Server.LOG.1", true)]
        [InlineData("log", MatchMode.PlainText, true, "Server.LOG.1", false)]
        [InlineData(@"^\d{4}-\d{2}\.txt$", MatchMode.Regex, false, "2024-05.txt", true)]
        [InlineData(@"^\d{4}-\d{2}\.txt$", MatchMode.Regex, false, "x2024-05.txt", false)]
        public void Matches(string pattern, MatchMode mode, bool caseSensitive, string name, bool expected)
        {
            var matcher = NameMatcher.Create(pattern, mode, caseSensitive);
            Assert.NotNull(matcher);
            Assert.Equal(expected, matcher.IsMatch(name));
        }

        [Fact]
        public void EmptyPatternMatchesEverything() => Assert.Null(NameMatcher.Create("  ", MatchMode.Wildcard, false));

        [Fact]
        public void BadRegexIsReported() => Assert.ThrowsAny<ArgumentException>(() => NameMatcher.Create("(", MatchMode.Regex, false));

        [Fact]
        public void ExcludeDoesNotWrapPlainNames()
        {
            var exclude = NameMatcher.Create("node_modules", MatchMode.Wildcard, false, wrapPlainWildcards: false)!;
            Assert.True(exclude.IsMatch("node_modules"));
            Assert.False(exclude.IsMatch("my_node_modules_backup"));
        }
    }

    public class ContentMatcherTests : IDisposable
    {
        private readonly string _folder = Directory.CreateTempSubdirectory("fs-content-").FullName;

        public void Dispose() => Directory.Delete(_folder, true);

        private string Write(string name, byte[] bytes)
        {
            var path = Path.Combine(_folder, name);
            File.WriteAllBytes(path, bytes);
            return path;
        }

        private static ContentOutcome Search(string path, string pattern, MatchMode mode, bool caseSensitive, out int line, out string snippet, bool binary = false)
            => ContentMatcher.Create(pattern, mode, caseSensitive).SearchFile(path, binary, CancellationToken.None, out line, out snippet);

        [Fact]
        public void PlainTextReportsLineAndSnippet()
        {
            var path = Write("a.txt", Encoding.UTF8.GetBytes("first\r\nsecond line\r\nthe NEEDLE is here\r\nlast"));
            Assert.Equal(ContentOutcome.Match, Search(path, "needle", MatchMode.PlainText, false, out var line, out var snippet));
            Assert.Equal(3, line);
            Assert.Equal("the NEEDLE is here", snippet);
        }

        [Fact]
        public void PlainTextCaseSensitive()
        {
            var path = Write("a.txt", Encoding.UTF8.GetBytes("the NEEDLE"));
            Assert.Equal(ContentOutcome.NoMatch, Search(path, "needle", MatchMode.PlainText, true, out _, out _));
        }

        [Fact]
        public void PlainTextFoundAcrossBlockBoundary()
        {
            // The block is 128K chars: put the text so that it straddles the first boundary, and check the line count survives it.
            var text = new StringBuilder();
            text.Append("line one\n");
            text.Append('x', 128 * 1024 - 9 - 3).Append('\n');
            text.Append("ab"); // starts before the boundary
            text.Append("cdef\nrest");
            var path = Write("big.txt", Encoding.UTF8.GetBytes(text.ToString()));
            Assert.Equal(ContentOutcome.Match, Search(path, "abcdef", MatchMode.PlainText, true, out var line, out _));
            Assert.Equal(3, line);
        }

        [Fact]
        public void PlainTextMayContainLineBreak()
        {
            var path = Write("a.txt", Encoding.UTF8.GetBytes("alpha\nbeta\ngamma"));
            Assert.Equal(ContentOutcome.Match, Search(path, "alpha\nbeta", MatchMode.PlainText, true, out var line, out _));
            Assert.Equal(1, line);
        }

        [Fact]
        public void RegexMatchesPerLine()
        {
            var path = Write("a.txt", Encoding.UTF8.GetBytes("nothing\nid=42;\nid=7;\n"));
            Assert.Equal(ContentOutcome.Match, Search(path, @"id=\d{2}", MatchMode.Regex, true, out var line, out var snippet));
            Assert.Equal(2, line);
            Assert.Equal("id=42;", snippet);
        }

        [Fact]
        public void RegexLineNumberAcrossBlocks()
        {
            var text = new StringBuilder();
            for (int i = 1; i <= 20000; i++)
            {
                text.Append(i == 15000 ? "target-line" : "filler line number " + i).Append("\r\n");
            }
            var path = Write("many.txt", Encoding.UTF8.GetBytes(text.ToString()));
            Assert.Equal(ContentOutcome.Match, Search(path, "^target", MatchMode.Regex, true, out var line, out _));
            Assert.Equal(15000, line);
        }

        [Fact]
        public void WildcardMatchesWithinALine()
        {
            var path = Write("a.txt", Encoding.UTF8.GetBytes("foo bar baz\nqux"));
            Assert.Equal(ContentOutcome.Match, Search(path, "foo*baz", MatchMode.Wildcard, true, out var line, out _));
            Assert.Equal(1, line);
            Assert.Equal(ContentOutcome.NoMatch, Search(path, "foo*qux", MatchMode.Wildcard, true, out _, out _));
        }

        [Fact]
        public void LastLineWithoutNewlineIsSearched()
        {
            var path = Write("a.txt", Encoding.UTF8.GetBytes("one\ntwo\nthree-x"));
            Assert.Equal(ContentOutcome.Match, Search(path, "three", MatchMode.Regex, true, out var line, out _));
            Assert.Equal(3, line);
        }

        [Fact]
        public void Utf16WithByteOrderMarkIsSearched()
        {
            var bytes = Encoding.Unicode.GetPreamble().Concat(Encoding.Unicode.GetBytes("héllo wörld\nsecond")).ToArray();
            var path = Write("u16.txt", bytes);
            Assert.Equal(ContentOutcome.Match, Search(path, "wörld", MatchMode.PlainText, true, out var line, out _));
            Assert.Equal(1, line);
        }

        [Fact]
        public void BinaryFilesAreSkippedUnlessAsked()
        {
            var bytes = new byte[] { 0x4D, 0x5A, 0x00, 0x00, 0x00, 0x01 }.Concat(Encoding.ASCII.GetBytes("SECRET")).ToArray();
            var path = Write("a.bin", bytes);
            Assert.Equal(ContentOutcome.SkippedBinary, Search(path, "SECRET", MatchMode.PlainText, true, out _, out _));
            Assert.Equal(ContentOutcome.Match, Search(path, "SECRET", MatchMode.PlainText, true, out _, out _, binary: true));
        }

        [Fact]
        public void FileOpenForWritingCanStillBeRead()
        {
            var path = Write("locked.txt", Encoding.UTF8.GetBytes("some text"));
            using var writer = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            Assert.Equal(ContentOutcome.Match, Search(path, "text", MatchMode.PlainText, true, out _, out _));
        }

        [Fact]
        public void FindAllLocatesEveryMatch()
        {
            var matcher = ContentMatcher.Create("ab", MatchMode.PlainText, true);
            Assert.Equal([(0, 2), (4, 2), (8, 2)], matcher.FindAll("ab  ab  ab", 10));
            Assert.Single(matcher.FindAll("ab  ab  ab", 1));
        }

        [Fact]
        public void BadRegexIsReported() => Assert.ThrowsAny<ArgumentException>(() => ContentMatcher.Create("[", MatchMode.Regex, true));
    }

    public class SearchSessionTests : IDisposable
    {
        private readonly string _root = Directory.CreateTempSubdirectory("fs-search-").FullName;

        public SearchSessionTests()
        {
            Make("a.txt", "hello world");
            Make("b.cs", "class B { string s = \"hello\"; }");
            Make("sub/c.txt", "nothing to see");
            Make("sub/deeper/d.log", "hello again\nsecond hello");
            Make("node_modules/pkg/e.txt", "hello from a package");
            Make("img/p.png", "not really a png");
            File.WriteAllBytes(Path.Combine(_root, "blob.dat"), [0, 1, 2, 0, 0x68, 0x65, 0x6C, 0x6C, 0x6F]);
        }

        public void Dispose() => Directory.Delete(_root, true);

        private void Make(string relative, string text)
        {
            var path = Path.Combine(_root, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, text);
        }

        private List<string> Run(Action<SearchCriteria> configure)
        {
            var criteria = new SearchCriteria { Locations = [_root] };
            configure(criteria);

            var session = new SearchSession(criteria);
            session.Start();
            Assert.True(session.Completion.Wait(TimeSpan.FromSeconds(30)));

            var results = new List<SearchResult>();
            session.Drain(results);
            return results.Select(r => Path.GetRelativePath(_root, r.FullPath).Replace('\\', '/')).Order().ToList();
        }

        [Fact]
        public void FindsEveryFileWithNoFilters()
            => Assert.Equal(7, Run(_ => { }).Count);

        [Fact]
        public void NameWildcard()
            => Assert.Equal(["a.txt", "node_modules/pkg/e.txt", "sub/c.txt"], Run(c => c.NamePattern = "*.txt"));

        [Fact]
        public void ExcludeFoldersAreNotEntered()
            => Assert.Equal(["a.txt", "sub/c.txt"], Run(c => { c.NamePattern = "*.txt"; c.Exclude = "node_modules"; }));

        [Fact]
        public void NoSubfolders()
            => Assert.Equal(["a.txt", "b.cs", "blob.dat"], Run(c => c.IncludeSubfolders = false));

        [Fact]
        public void CategoryFiltersByExtension()
            => Assert.Equal(["img/p.png"], Run(c => c.Category = FileCategory.Images));

        [Fact]
        public void SizeFilter()
            => Assert.Equal(["blob.dat"], Run(c => c.MaxSize = 9));

        [Fact]
        public void DateFilter()
            => Assert.Empty(Run(c => c.ModifiedBefore = DateTime.Now.AddDays(-1)));

        [Fact]
        public void ContentPlainText()
            => Assert.Equal(["a.txt", "b.cs", "node_modules/pkg/e.txt", "sub/deeper/d.log"], Run(c => c.ContentPattern = "HELLO"));

        [Fact]
        public void ContentCombinedWithName()
            => Assert.Equal(["a.txt"], Run(c => { c.ContentPattern = "hello"; c.NamePattern = "*.txt"; c.Exclude = "node_modules"; }));

        [Fact]
        public void ContentRegex()
            => Assert.Equal(["sub/deeper/d.log"], Run(c => { c.ContentPattern = @"hello\s+again"; c.ContentMode = MatchMode.Regex; }));

        [Fact]
        public void ContentInBinaryFilesOnlyWhenAsked()
        {
            Assert.DoesNotContain("blob.dat", Run(c => c.ContentPattern = "hello"));
            Assert.Contains("blob.dat", Run(c => { c.ContentPattern = "hello"; c.SearchBinaryFiles = true; }));
        }

        [Fact]
        public void FoldersCanBeFound()
            => Assert.Equal(["node_modules", "sub"], Run(c => { c.Kinds = ItemKinds.Folders; c.NamePattern = "*u*"; })
                .Where(p => !p.Contains('/')).ToList());

        [Fact]
        public void ContentResultsCarryLineAndSnippet()
        {
            var criteria = new SearchCriteria { Locations = [_root], ContentPattern = "second", NamePattern = "*.log" };
            var session = new SearchSession(criteria);
            session.Start();
            session.Completion.Wait();
            var results = new List<SearchResult>();
            session.Drain(results);
            var result = Assert.Single(results);
            Assert.Equal(2, result.Line);
            Assert.Equal("second hello", result.Snippet);
        }

        [Fact]
        public void CancelStopsTheSearch()
        {
            var session = new SearchSession(new SearchCriteria { Locations = [_root] });
            session.Start();
            session.Cancel();
            Assert.True(session.Completion.Wait(TimeSpan.FromSeconds(30)));
            Assert.False(session.IsRunning);
        }

        [Fact]
        public void InvalidPatternsThrowBeforeStarting()
        {
            Assert.Throws<ArgumentException>(() => new SearchSession(new SearchCriteria { Locations = [_root], NamePattern = "(", NameMode = MatchMode.Regex }));
            Assert.Throws<ArgumentException>(() => new SearchSession(new SearchCriteria { Locations = [_root], ContentPattern = "[", ContentMode = MatchMode.Regex }));
        }

        [Fact]
        public void MissingLocationDoesNotHang()
        {
            var session = new SearchSession(new SearchCriteria { Locations = [Path.Combine(_root, "does-not-exist")] });
            session.Start();
            Assert.True(session.Completion.Wait(TimeSpan.FromSeconds(30)));
        }
    }
}
