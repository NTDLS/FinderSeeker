using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Enumeration;
using System.Threading.Channels;

namespace FinderSeeker.Search
{
    /// <summary>
    /// One file or folder that was found.
    /// </summary>
    public sealed class SearchResult
    {
        public required string Name { get; init; }
        public required string Folder { get; init; }
        public required bool IsDirectory { get; init; }
        public long Length { get; init; }
        public DateTime Modified { get; init; }
        public DateTime Created { get; init; }

        /// <summary>For a content search, the line of the first match (starting at 1), otherwise zero.</summary>
        public int Line { get; init; }

        /// <summary>For a content search, the text around the first match.</summary>
        public string? Snippet { get; init; }

        public string FullPath => Path.Join(Folder, Name);
    }

    /// <summary>
    /// A running (or finished) search. Folders are walked by several workers at once; each worker lists a folder with the
    /// operating system's bulk enumeration, so the names, sizes and dates come back in one call per folder, and a file that does
    /// not match is rejected without anything being allocated for it. Results are queued as they are found, to be drained by the UI.
    /// </summary>
    public sealed class SearchSession
    {
        private readonly ConcurrentQueue<SearchResult> _results = new();

        private readonly SearchCriteria _criteria;
        private readonly CancellationTokenSource _cancellation = new();
        private readonly Stopwatch _stopwatch = new();

        private readonly NameMatcher? _name;
        private readonly NameMatcher? _exclude;
        private readonly ContentMatcher? _content;
        private readonly HashSet<string>? _extensions;
        private readonly bool _wantFiles;
        private readonly bool _wantFolders;
        private readonly bool _recurse;
        private readonly long _modifiedAfter, _modifiedBefore, _createdAfter, _createdBefore;
        private readonly long _minSize, _maxSize;

        private Channel<string>? _folders;
        private int _pendingFolders;
        private long _filesScanned, _foldersScanned, _found, _contentBytes, _contentFilesRead, _errors, _skippedBinary;

        /// <summary>
        /// Prepares a search.
        /// </summary>
        /// <exception cref="ArgumentException">A pattern is not valid (the message says which).</exception>
        public SearchSession(SearchCriteria criteria)
        {
            _criteria = criteria;

            try
            {
                _name = NameMatcher.Create(criteria.NamePattern, criteria.NameMode, criteria.NameCaseSensitive);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(Localize("The file name pattern is not valid: {0}", ex.Message), ex);
            }

            _exclude = NameMatcher.Create(criteria.Exclude, MatchMode.Wildcard, false, wrapPlainWildcards: false);

            try
            {
                if (criteria.HasContentSearch)
                {
                    _content = ContentMatcher.Create(criteria.ContentPattern, criteria.ContentMode, criteria.ContentCaseSensitive);
                }
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(Localize("The text to find is not valid: {0}", ex.Message), ex);
            }

            var extensions = FileCategories.ExtensionsOf(criteria.Category);
            if (extensions.Length > 0)
            {
                _extensions = new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase);
            }

            _wantFiles = criteria.FilesOnly || criteria.Kinds != ItemKinds.Folders;
            _wantFolders = criteria.Kinds != ItemKinds.Files && !criteria.FilesOnly;
            _recurse = criteria.IncludeSubfolders;

            _modifiedAfter = ToUtcTicks(criteria.ModifiedAfter, long.MinValue);
            _modifiedBefore = ToUtcTicks(criteria.ModifiedBefore, long.MaxValue);
            _createdAfter = ToUtcTicks(criteria.CreatedAfter, long.MinValue);
            _createdBefore = ToUtcTicks(criteria.CreatedBefore, long.MaxValue);
            _minSize = criteria.MinSize ?? long.MinValue;
            _maxSize = criteria.MaxSize ?? long.MaxValue;
        }

        private static long ToUtcTicks(DateTime? local, long fallback)
            => local == null ? fallback : new DateTimeOffset(DateTime.SpecifyKind(local.Value, DateTimeKind.Local)).UtcTicks;

        #region Progress.

        public long FilesScanned => Interlocked.Read(ref _filesScanned);
        public long FoldersScanned => Interlocked.Read(ref _foldersScanned);
        public long Found => Interlocked.Read(ref _found);

        /// <summary>Bytes of file content that were searched.</summary>
        public long ContentBytes => Interlocked.Read(ref _contentBytes);
        public long ContentFilesRead => Interlocked.Read(ref _contentFilesRead);
        public long SkippedBinary => Interlocked.Read(ref _skippedBinary);

        /// <summary>Files that could not be read.</summary>
        public long Errors => Interlocked.Read(ref _errors);

        /// <summary>The content search, for highlighting matches elsewhere. Null when the contents are not being searched.</summary>
        public ContentMatcher? Content => _content;

        public TimeSpan Elapsed => _stopwatch.Elapsed;
        public bool IsRunning { get; private set; }

        public bool WasCancelled => _cancellation.IsCancellationRequested;

        /// <summary>Completes when the search has finished or been stopped.</summary>
        public Task Completion { get; private set; } = Task.CompletedTask;

        /// <summary>
        /// Takes the results found so far (those not taken before).
        /// </summary>
        public int Drain(List<SearchResult> into, int limit = int.MaxValue)
        {
            int count = 0;
            while (count < limit && _results.TryDequeue(out var result))
            {
                into.Add(result);
                count++;
            }
            return count;
        }

        #endregion

        public void Cancel() => _cancellation.Cancel();

        /// <summary>
        /// Starts the search in the background.
        /// </summary>
        public void Start()
        {
            IsRunning = true;
            _stopwatch.Start();

            _folders = Channel.CreateUnbounded<string>(new UnboundedChannelOptions { SingleWriter = false, SingleReader = false });

            foreach (var location in _criteria.Locations)
            {
                Enqueue(location);
            }

            if (Volatile.Read(ref _pendingFolders) == 0)
            {
                _folders.Writer.TryComplete();
            }

            int workers = Math.Max(4, Environment.ProcessorCount);
            var tasks = new Task[workers];
            for (int i = 0; i < workers; i++)
            {
                tasks[i] = Task.Factory.StartNew(Work, TaskCreationOptions.LongRunning);
            }

            Completion = Task.WhenAll(tasks).ContinueWith(_ =>
            {
                _stopwatch.Stop();
                IsRunning = false;
            }, TaskScheduler.Default);
        }

        private void Enqueue(string folder)
        {
            Interlocked.Increment(ref _pendingFolders);
            if (!_folders!.Writer.TryWrite(folder))
            {
                FolderDone();
            }
        }

        private void FolderDone()
        {
            if (Interlocked.Decrement(ref _pendingFolders) == 0)
            {
                _folders!.Writer.TryComplete();
            }
        }

        #region Folder listing.

        /// <summary>
        /// What the listing of a folder says about one entry.
        /// </summary>
        private struct Entry
        {
            public string Name;
            public bool IsDirectory;
            public bool Emit;
            public bool Traverse;
            public long Length;
            public DateTime Modified;
            public DateTime Created;
        }

        /// <summary>
        /// Filters and converts the entries of folder listings. One per worker, since it carries the verdict from the filter to the conversion.
        /// </summary>
        private sealed class Lister
        {
            private readonly SearchSession _s;
            private bool _emit, _traverse;

            public Lister(SearchSession session)
            {
                _s = session;
            }

            public bool Include(ref FileSystemEntry entry)
            {
                var s = _s;
                var c = s._criteria;
                var name = entry.FileName;
                var attributes = entry.Attributes;

                bool hidden = (attributes & FileAttributes.Hidden) != 0;
                bool system = (attributes & FileAttributes.System) != 0;
                bool visible = (c.IncludeHidden || !hidden) && (c.IncludeSystem || !system);

                _emit = false;
                _traverse = false;

                if (!visible || s._exclude != null && s._exclude.IsMatch(name))
                {
                    return false;
                }

                if (entry.IsDirectory)
                {
                    //A junction or a symbolic link is listed but not entered: it may lead back to where we came from.
                    _traverse = s._recurse && (attributes & FileAttributes.ReparsePoint) == 0;

                    if (s._wantFolders
                        && s.PassesDates(ref entry)
                        && (s._name == null || c.NameMatchesFullPath || s._name.IsMatch(name)))
                    {
                        _emit = true;
                    }

                    return _traverse || _emit;
                }

                if (!s._wantFiles)
                {
                    return false;
                }

                if (entry.Length < s._minSize || entry.Length > s._maxSize || !s.PassesDates(ref entry))
                {
                    return false;
                }

                if (s._extensions != null
                    && !s._extensions.GetAlternateLookup<ReadOnlySpan<char>>().Contains(Path.GetExtension(name)))
                {
                    return false;
                }

                if (s._name != null && !c.NameMatchesFullPath && !s._name.IsMatch(name))
                {
                    return false;
                }

                _emit = true;
                return true;
            }

            public Entry Transform(ref FileSystemEntry entry)
            {
                return new Entry
                {
                    Name = entry.FileName.ToString(),
                    IsDirectory = entry.IsDirectory,
                    Emit = _emit,
                    Traverse = _traverse,
                    Length = entry.IsDirectory ? 0 : entry.Length,
                    Modified = entry.LastWriteTimeUtc.LocalDateTime,
                    Created = entry.CreationTimeUtc.LocalDateTime
                };
            }
        }

        private bool PassesDates(ref FileSystemEntry entry)
        {
            long modified = entry.LastWriteTimeUtc.UtcTicks;
            if (modified < _modifiedAfter || modified > _modifiedBefore)
            {
                return false;
            }

            long created = entry.CreationTimeUtc.UtcTicks;
            return created >= _createdAfter && created <= _createdBefore;
        }

        #endregion

        private void Work()
        {
            var token = _cancellation.Token;
            var lister = new Lister(this);
            var options = new EnumerationOptions
            {
                RecurseSubdirectories = false,
                IgnoreInaccessible = true,
                AttributesToSkip = 0,
                ReturnSpecialDirectories = false,
                BufferSize = 64 * 1024
            };

            var reader = _folders!.Reader;

            try
            {
                while (reader.WaitToReadAsync(token).AsTask().GetAwaiter().GetResult())
                {
                    while (reader.TryRead(out var folder))
                    {
                        try
                        {
                            if (!token.IsCancellationRequested)
                            {
                                ScanFolder(folder, lister, options, token);
                            }
                        }
                        finally
                        {
                            FolderDone();
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                //Stopped by the user.
            }
        }

        private void ScanFolder(string folder, Lister lister, EnumerationOptions options, CancellationToken token)
        {
            Interlocked.Increment(ref _foldersScanned);

            try
            {
                var listing = new FileSystemEnumerable<Entry>(folder, lister.Transform, options)
                {
                    ShouldIncludePredicate = lister.Include
                };

                foreach (var entry in listing)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    if (entry.IsDirectory)
                    {
                        if (entry.Traverse)
                        {
                            Enqueue(Path.Join(folder, entry.Name));
                        }

                        if (entry.Emit && PassesFullPath(folder, entry.Name))
                        {
                            Report(folder, entry, 0, null);
                        }
                    }
                    else
                    {
                        Interlocked.Increment(ref _filesScanned);

                        if (entry.Emit)
                        {
                            ConsiderFile(folder, entry, token);
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException)
            {
                //The folder vanished or cannot be listed. There is nothing in it to find.
                Interlocked.Increment(ref _errors);
            }
        }

        private bool PassesFullPath(string folder, string name)
        {
            if (_name == null || !_criteria.NameMatchesFullPath)
            {
                return true;
            }

            return _name.IsMatch(Path.Join(folder, name));
        }

        private void ConsiderFile(string folder, Entry entry, CancellationToken token)
        {
            if (!PassesFullPath(folder, entry.Name))
            {
                return;
            }

            if (_content == null)
            {
                Report(folder, entry, 0, null);
                return;
            }

            if (_criteria.MaxContentFileSize > 0 && entry.Length > _criteria.MaxContentFileSize)
            {
                return;
            }

            if (entry.Length == 0)
            {
                return;
            }

            var outcome = _content.SearchFile(Path.Join(folder, entry.Name), _criteria.SearchBinaryFiles, token, out var line, out var snippet);

            switch (outcome)
            {
                case ContentOutcome.SkippedBinary:
                    Interlocked.Increment(ref _skippedBinary);
                    return;
                case ContentOutcome.Error:
                    Interlocked.Increment(ref _errors);
                    return;
            }

            Interlocked.Increment(ref _contentFilesRead);
            Interlocked.Add(ref _contentBytes, entry.Length);

            if (outcome == ContentOutcome.Match)
            {
                Report(folder, entry, line, snippet);
            }
        }

        private void Report(string folder, Entry entry, int line, string? snippet)
        {
            Interlocked.Increment(ref _found);
            _results.Enqueue(new SearchResult
            {
                Name = entry.Name,
                Folder = folder,
                IsDirectory = entry.IsDirectory,
                Length = entry.Length,
                Modified = entry.Modified,
                Created = entry.Created,
                Line = line,
                Snippet = snippet
            });
        }
    }
}
