using FinderSeeker.Localization;
using FinderSeeker.Search;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FinderSeeker.UI
{
    /// <summary>
    /// The main window: the search criteria at the top, the results below, and a preview of the selected result on the right.
    /// The layout is in FormMain.Designer.cs (open the form in the Visual Studio designer to change it). The text the designer puts
    /// on controls is translated by <see cref="FormLocalizer"/>, so it is also the English phrase in the translation database.
    /// </summary>
    public partial class FormMain : Form
    {
        private sealed class Choice<T>(T value, Func<string> text)
        {
            public T Value { get; } = value;
            public override string ToString() => text();
        }

        private enum State { Ready, Running, Done, Stopped }

        private const int HistoryLimit = 20;

        private readonly AppSettings _settings;
        private readonly List<Action> _retext = new();
        private readonly ShellIcons _icons = new();
        private readonly List<SearchResult> _results = new();
        private int _sortColumn = 1;
        private bool _sortAscending = true;

        private SearchSession? _session;
        private State _state = State.Ready;
        private bool _stopRequested;
        private bool _loaded;
        private bool _sorting;

        public FormMain()
        {
            InitializeComponent();

            _settings = AppSettings.Load();

            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            // Choices that follow the language, and the pieces that need code rather than the designer.
            FillChoices(comboNameMode,
                (MatchMode.Wildcard, () => Localize("Wildcards")),
                (MatchMode.PlainText, () => Localize("Plain text")),
                (MatchMode.Regex, () => Localize("Regular expression")));
            FillChoices(comboContentMode,
                (MatchMode.PlainText, () => Localize("Plain text")),
                (MatchMode.Wildcard, () => Localize("Wildcards")),
                (MatchMode.Regex, () => Localize("Regular expression")));
            foreach (var unit in new[] { comboMinUnit, comboMaxUnit })
            {
                FillChoices(unit,
                    (0, () => Localize("bytes")),
                    (1, () => Localize("KB")),
                    (2, () => Localize("MB")),
                    (3, () => Localize("GB")));
            }

            listResults.SmallImageList = _icons.Images;

            ApplySettings();
            ApplyLanguage();

            // Set after the first translation pass, so these are not treated as phrases to translate.
            Text = "FinderSeeker";
            checkNameCase.Text = checkContentCase.Text = "Aa";

            Theming.ApplyDarkModeFixups(this);
        }

        #region Events.

        private void buttonSearch_Click(object? sender, EventArgs e) => ToggleSearch();
        private void buttonBrowse_Click(object? sender, EventArgs e) => Browse();
        private void menuExit_Click(object? sender, EventArgs e) => Close();
        private void menuOpenLog_Click(object? sender, EventArgs e) => OpenLogFolder();
        private void menuAbout_Click(object? sender, EventArgs e) => ShowAbout();
        private void menuShowPreview_CheckedChanged(object? sender, EventArgs e) => SetPreviewVisible(menuShowPreview.Checked);
        private void checkUseSize_CheckedChanged(object? sender, EventArgs e) => UpdateEnabled();
        private void timerUi_Tick(object? sender, EventArgs e) => OnTick();

        private void timerPreview_Tick(object? sender, EventArgs e)
        {
            timerPreview.Stop();
            previewPanel.ShowResult(SelectedResult());
        }

        private void listResults_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e) => OnRetrieveItem(sender, e);
        private void listResults_ColumnClick(object? sender, ColumnClickEventArgs e) => SortBy(e.Column);
        private void listResults_ItemActivate(object? sender, EventArgs e) => OpenSelected();
        private void listResults_KeyDown(object? sender, KeyEventArgs e) => OnListKeyDown(sender, e);

        private void listResults_SelectedIndexChanged(object? sender, EventArgs e)
        {
            timerPreview.Stop();
            timerPreview.Start();
        }

        private void contextMenuResults_Opening(object? sender, CancelEventArgs e) => e.Cancel = listResults.SelectedIndices.Count == 0;
        private void menuOpen_Click(object? sender, EventArgs e) => OpenSelected();
        private void menuShowInExplorer_Click(object? sender, EventArgs e) => ShowSelectedInExplorer();
        private void menuCopyPath_Click(object? sender, EventArgs e) => CopySelected(r => r.FullPath);
        private void menuCopyName_Click(object? sender, EventArgs e) => CopySelected(r => r.Name);
        private void menuCopyFolder_Click(object? sender, EventArgs e) => CopySelected(r => r.Folder);

        #endregion

        #region Language.

        private void menuLanguage_Click(object? sender, EventArgs e)
        {
            using var form = new FormLanguage(_settings.Language) { Icon = Icon };
            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            _settings.Language = form.SelectedLanguage;
            Localizer.Default.SetLanguage(_settings.Language);
            _settings.Save();
            ApplyLanguage();
        }

        /// <summary>
        /// Puts all of the text on the window, in the current language. Called at the start and whenever the language changes.
        /// The text the designer put on the controls is translated by <see cref="FormLocalizer"/>; the rest is set here.
        /// </summary>
        private void ApplyLanguage()
        {
            FormLocalizer.Apply(this);

            foreach (var apply in _retext)
            {
                apply();
            }

            toolTip.SetToolTip(comboNameMode, Localize("How the name is matched. With wildcards, * is any text and ? is any character, and several patterns can be separated with a semicolon (*.cs;*.vb). A pattern with no wildcard finds names containing it."));
            toolTip.SetToolTip(comboContentMode, Localize("How the text inside files is matched. Wildcard and regular expression patterns are matched within a single line."));
            toolTip.SetToolTip(checkNameCase, Localize("Match case"));
            toolTip.SetToolTip(checkContentCase, Localize("Match case"));
            toolTip.SetToolTip(numMaxContentMb, Localize("Files larger than this many megabytes are not searched for text. Zero means no limit."));
            toolTip.SetToolTip(comboLocation, Localize("Folders or drives to search, separated by semicolons."));
            toolTip.SetToolTip(checkBinary, Localize("Also look inside files that do not look like text. They are read as single-byte text."));

            UpdateColumnHeaders();
            previewPanel.ShowResult(SelectedResult());
            UpdateSearchButton();
            UpdateStatus();
        }

        /// <summary>
        /// Fills a drop-down list with choices whose text follows the language.
        /// </summary>
        private void FillChoices<T>(ComboBox combo, params (T Value, Func<string> Text)[] items)
        {
            void Fill()
            {
                int selected = Math.Max(combo.SelectedIndex, 0);
                combo.BeginUpdate();
                combo.Items.Clear();
                foreach (var (value, text) in items)
                {
                    combo.Items.Add(new Choice<T>(value, text));
                }
                combo.SelectedIndex = selected;
                combo.EndUpdate();
            }

            Fill();
            _retext.Add(Fill);
        }

        private static T ValueOf<T>(ComboBox combo) => ((Choice<T>)combo.SelectedItem!).Value;

        private static void Pick<T>(ComboBox combo, T value)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(((Choice<T>)combo.Items[i]!).Value, value))
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        private void UpdateColumnHeaders()
        {
            for (int i = 0; i < listResults.Columns.Count; i++)
            {
                var column = listResults.Columns[i];
                var arrow = i == _sortColumn ? (_sortAscending ? "  ▲" : "  ▼") : string.Empty;
                column.Text = Localize(FormLocalizer.SourceText(column) ?? column.Text) + arrow;
            }
        }

        #endregion

        #region Settings.

        private void ApplySettings()
        {
            var s = _settings;

            comboLocation.Items.Add(Localize("All local drives"));
            foreach (var drive in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed))
            {
                comboLocation.Items.Add(drive.Name);
            }
            foreach (var item in s.LocationHistory)
            {
                comboLocation.Items.Add(item);
            }
            comboName.Items.AddRange(s.NameHistory.ToArray());
            comboContent.Items.AddRange(s.ContentHistory.ToArray());

            comboLocation.Text = string.IsNullOrWhiteSpace(s.Location) ? (Path.GetPathRoot(Environment.SystemDirectory) ?? string.Empty) : s.Location;
            checkSubfolders.Checked = s.IncludeSubfolders;
            comboName.Text = s.Name;
            Pick(comboNameMode, s.NameMode);
            checkNameCase.Checked = s.NameCaseSensitive;
            checkHidden.Checked = s.IncludeHidden;
            checkSystem.Checked = s.IncludeSystem;

            comboContent.Text = s.Content;
            Pick(comboContentMode, s.ContentMode);
            checkContentCase.Checked = s.ContentCaseSensitive;
            checkBinary.Checked = s.SearchBinaryFiles;
            numMaxContentMb.Value = Math.Clamp(s.MaxContentFileSizeMb, numMaxContentMb.Minimum, numMaxContentMb.Maximum);

            checkUseMin.Checked = s.UseMinSize;
            numMin.Value = Math.Clamp(s.MinSize, numMin.Minimum, numMin.Maximum);
            Pick(comboMinUnit, s.MinSizeUnit);
            checkUseMax.Checked = s.UseMaxSize;
            numMax.Value = Math.Clamp(s.MaxSize, numMax.Minimum, numMax.Maximum);
            Pick(comboMaxUnit, s.MaxSizeUnit);

            SetDate(dateModifiedAfter, s.ModifiedAfter);
            SetDate(dateModifiedBefore, s.ModifiedBefore);
            SetDate(dateCreatedAfter, s.CreatedAfter);
            SetDate(dateCreatedBefore, s.CreatedBefore);

            menuShowPreview.Checked = s.ShowPreview;
            SetPreviewVisible(s.ShowPreview);

            if (s.WindowBounds is { } bounds && Screen.AllScreens.Any(screen => screen.WorkingArea.IntersectsWith(bounds)))
            {
                StartPosition = FormStartPosition.Manual;
                Bounds = bounds;
            }
            if (s.WindowMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }

            UpdateEnabled();
        }

        private static void SetDate(DateTimePicker picker, DateTime? value)
        {
            if (value != null)
            {
                picker.Value = value.Value;
                picker.Checked = true;
            }
            else
            {
                picker.Value = DateTime.Now;
                picker.Checked = false;
            }
        }

        private static DateTime? GetDate(DateTimePicker picker) => picker.Checked ? picker.Value : null;

        private void UpdateEnabled()
        {
            numMin.Enabled = comboMinUnit.Enabled = checkUseMin.Checked;
            numMax.Enabled = comboMaxUnit.Enabled = checkUseMax.Checked;
        }

        private void CaptureSettings()
        {
            var s = _settings;

            s.Location = comboLocation.Text;
            s.IncludeSubfolders = checkSubfolders.Checked;
            s.Name = comboName.Text;
            s.NameMode = ValueOf<MatchMode>(comboNameMode);
            s.NameCaseSensitive = checkNameCase.Checked;
            s.IncludeHidden = checkHidden.Checked;
            s.IncludeSystem = checkSystem.Checked;

            s.Content = comboContent.Text;
            s.ContentMode = ValueOf<MatchMode>(comboContentMode);
            s.ContentCaseSensitive = checkContentCase.Checked;
            s.SearchBinaryFiles = checkBinary.Checked;
            s.MaxContentFileSizeMb = numMaxContentMb.Value;

            s.UseMinSize = checkUseMin.Checked;
            s.MinSize = numMin.Value;
            s.MinSizeUnit = ValueOf<int>(comboMinUnit);
            s.UseMaxSize = checkUseMax.Checked;
            s.MaxSize = numMax.Value;
            s.MaxSizeUnit = ValueOf<int>(comboMaxUnit);

            s.ModifiedAfter = GetDate(dateModifiedAfter);
            s.ModifiedBefore = GetDate(dateModifiedBefore);
            s.CreatedAfter = GetDate(dateCreatedAfter);
            s.CreatedBefore = GetDate(dateCreatedBefore);

            s.ShowPreview = menuShowPreview.Checked;
            if (!splitMain.Panel2Collapsed)
            {
                s.PreviewWidth = splitMain.Panel2.Width;
            }

            s.WindowMaximized = WindowState == FormWindowState.Maximized;
            s.WindowBounds = WindowState == FormWindowState.Normal ? Bounds : RestoreBounds;
        }

        private static void Remember(List<string> history, ComboBox box, string text)
        {
            text = text.Trim();
            if (text.Length == 0)
            {
                return;
            }

            history.RemoveAll(h => h.Equals(text, StringComparison.OrdinalIgnoreCase));
            history.Insert(0, text);
            if (history.Count > HistoryLimit)
            {
                history.RemoveRange(HistoryLimit, history.Count - HistoryLimit);
            }

            int existing = box.Items.IndexOf(text);
            if (existing >= 0 && box.DropDownStyle == ComboBoxStyle.DropDown)
            {
                // Keep the typed text while moving it to the top of the list.
                box.Items.RemoveAt(existing);
            }
            box.Items.Insert(0, text);
            box.Text = text;
        }

        #endregion

        #region Searching.

        private void ToggleSearch()
        {
            if (_state == State.Running)
            {
                _stopRequested = true;
                _session?.Cancel();
                UpdateSearchButton();
                return;
            }

            StartSearch();
        }

        private SearchCriteria? BuildCriteria()
        {
            var locations = new List<string>();
            foreach (var part in comboLocation.Text.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (part.Equals(Localize("All local drives"), StringComparison.CurrentCultureIgnoreCase) || part.Equals("All local drives", StringComparison.OrdinalIgnoreCase))
                {
                    locations.AddRange(DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed && d.IsReady).Select(d => d.RootDirectory.FullName));
                }
                else
                {
                    var path = Environment.ExpandEnvironmentVariables(part);
                    try { path = Path.GetFullPath(path); } catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException) { }
                    if (!Directory.Exists(path))
                    {
                        MessageBox.Show(this, Localize("The folder \"{0}\" does not exist.", part), "FinderSeeker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        comboLocation.Focus();
                        return null;
                    }
                    locations.Add(path);
                }
            }

            if (locations.Count == 0)
            {
                MessageBox.Show(this, Localize("Choose a folder to search in."), "FinderSeeker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboLocation.Focus();
                return null;
            }

            static long Bytes(decimal value, int unit) => (long)(value * (decimal)Math.Pow(1024, unit));

            return new SearchCriteria
            {
                Locations = locations,
                IncludeSubfolders = checkSubfolders.Checked,
                NamePattern = comboName.Text.Trim(),
                NameMode = ValueOf<MatchMode>(comboNameMode),
                NameCaseSensitive = checkNameCase.Checked,
                MinSize = checkUseMin.Checked ? Bytes(numMin.Value, ValueOf<int>(comboMinUnit)) : null,
                MaxSize = checkUseMax.Checked ? Bytes(numMax.Value, ValueOf<int>(comboMaxUnit)) : null,
                ModifiedAfter = GetDate(dateModifiedAfter),
                ModifiedBefore = GetDate(dateModifiedBefore),
                CreatedAfter = GetDate(dateCreatedAfter),
                CreatedBefore = GetDate(dateCreatedBefore),
                IncludeHidden = checkHidden.Checked,
                IncludeSystem = checkSystem.Checked,
                ContentPattern = comboContent.Text,
                ContentMode = ValueOf<MatchMode>(comboContentMode),
                ContentCaseSensitive = checkContentCase.Checked,
                SearchBinaryFiles = checkBinary.Checked,
                MaxContentFileSize = Bytes(numMaxContentMb.Value, 2)
            };
        }

        private void StartSearch()
        {
            var criteria = BuildCriteria();
            if (criteria == null)
            {
                return;
            }

            SearchSession session;
            try
            {
                session = new SearchSession(criteria);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "FinderSeeker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Remember(_settings.LocationHistory, comboLocation, comboLocation.Text);
            Remember(_settings.NameHistory, comboName, comboName.Text);
            Remember(_settings.ContentHistory, comboContent, comboContent.Text);
            CaptureSettings();
            _settings.Save();

            _results.Clear();
            listResults.VirtualListSize = 0;
            previewPanel.Highlighter = session.Content;
            previewPanel.ShowResult(null);

            _session = session;
            _stopRequested = false;
            _state = State.Running;
            progressSearch.Visible = true;
            session.Start();
            timerUi.Start();

            UpdateSearchButton();
            UpdateStatus();
        }

        private void OnTick()
        {
            var session = _session;
            if (session == null)
            {
                timerUi.Stop();
                return;
            }

            int before = _results.Count;
            int drained = session.Drain(_results, 50000);

            if (drained > 0)
            {
                listResults.VirtualListSize = _results.Count;
                if (before == 0 && listResults.SelectedIndices.Count == 0 && _results.Count > 0)
                {
                    listResults.EnsureVisible(0);
                }
            }

            if (!session.IsRunning && session.Completion.IsCompleted && drained == 0)
            {
                Finish(session);
                return;
            }

            UpdateStatus();
        }

        private void Finish(SearchSession session)
        {
            timerUi.Stop();
            _state = _stopRequested ? State.Stopped : State.Done;
            progressSearch.Visible = false;

            SortResults();
            UpdateSearchButton();
            UpdateStatus();
        }

        private void UpdateSearchButton()
        {
            if (_state == State.Running)
            {
                buttonSearch.Text = _stopRequested ? Localize("Stopping...") : Localize("Stop");
                buttonSearch.Enabled = !_stopRequested;
            }
            else
            {
                buttonSearch.Text = Localize("Search");
                buttonSearch.Enabled = true;
            }
        }

        private void UpdateStatus()
        {
            var session = _session;

            labelState.Text = _sorting ? Localize("Sorting...") : _state switch
            {
                State.Running => Localize("Searching..."),
                State.Done => Localize("Done"),
                State.Stopped => Localize("Stopped"),
                _ => Localize("Ready")
            };

            if (session == null)
            {
                labelStats.Text = string.Empty;
                labelTime.Text = string.Empty;
                return;
            }

            var stats = Localize("{0} found", session.Found.ToString("N0"))
                + "   |   " + Localize("{0} files in {1} folders checked", session.FilesScanned.ToString("N0"), session.FoldersScanned.ToString("N0"));

            if (session.ContentBytes > 0)
            {
                stats += "   |   " + Localize("{0} of text searched", Format.FileSize(session.ContentBytes));
            }
            if (session.SkippedBinary > 0)
            {
                stats += "   |   " + Localize("{0} binary files skipped", session.SkippedBinary.ToString("N0"));
            }
            if (session.Errors > 0)
            {
                stats += "   |   " + Localize("{0} could not be read", session.Errors.ToString("N0"));
            }

            labelStats.Text = stats;
            labelTime.Text = session.Elapsed.TotalSeconds < 60
                ? $"{session.Elapsed.TotalSeconds:N1} s"
                : session.Elapsed.ToString(@"h\:mm\:ss");
        }

        #endregion

        #region Results.

        private void OnRetrieveItem(object? sender, RetrieveVirtualItemEventArgs e)
        {
            var r = _results[e.ItemIndex];

            var item = new ListViewItem(r.Name, _icons.IndexFor(r.Name));
            item.SubItems.Add(r.Folder);
            item.SubItems.Add(Format.FileSize(r.Length));
            item.SubItems.Add(r.Modified.ToString("yyyy-MM-dd HH:mm"));
            item.SubItems.Add(r.Created.ToString("yyyy-MM-dd HH:mm"));
            item.SubItems.Add(r.Line > 0 ? $"{r.Line}: {r.Snippet}" : string.Empty);
            e.Item = item;
        }

        private void SortBy(int column)
        {
            if (column == _sortColumn)
            {
                _sortAscending = !_sortAscending;
            }
            else
            {
                _sortColumn = column;
                _sortAscending = column is not (3 or 4);
            }

            SortResults();
        }

        private async void SortResults()
        {
            if (_sorting)
            {
                return;
            }

            _sorting = true;
            try
            {
                var selected = SelectedResult();
                int direction = _sortAscending ? 1 : -1;
                int column = _sortColumn;

                static int ByPath(SearchResult a, SearchResult b)
                {
                    int c = string.Compare(a.Folder, b.Folder, StringComparison.OrdinalIgnoreCase);
                    return c != 0 ? c : string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
                }

                Comparison<SearchResult> compare = column switch
                {
                    0 => (a, b) => { int c = string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase); return c != 0 ? c : ByPath(a, b); },
                    2 => (a, b) => { int c = a.Length.CompareTo(b.Length); return c != 0 ? c : ByPath(a, b); },
                    3 => (a, b) => { int c = a.Modified.CompareTo(b.Modified); return c != 0 ? c : ByPath(a, b); },
                    4 => (a, b) => { int c = a.Created.CompareTo(b.Created); return c != 0 ? c : ByPath(a, b); },
                    5 => (a, b) => { int c = a.Line.CompareTo(b.Line); return c != 0 ? c : ByPath(a, b); },
                    _ => ByPath
                };

                // Sort a copy off the UI thread so a large result list does not freeze the window. Results that arrive meanwhile
                // are only ever added to the end, so they are kept after the sorted part.
                int count = _results.Count;
                var copy = _results.GetRange(0, count).ToArray();
                UpdateStatus();
                await Task.Run(() => Array.Sort(copy, (a, b) => compare(a, b) * direction));

                _results.RemoveRange(0, count);
                _results.InsertRange(0, copy);

                listResults.SelectedIndices.Clear();
                listResults.Invalidate();
                UpdateColumnHeaders();

                if (selected != null)
                {
                    int index = _results.IndexOf(selected);
                    if (index >= 0)
                    {
                        listResults.SelectedIndices.Add(index);
                        listResults.EnsureVisible(index);
                    }
                }
            }
            finally
            {
                _sorting = false;
                UpdateStatus();
            }
        }

        private SearchResult? SelectedResult()
        {
            if (listResults.SelectedIndices.Count == 0)
            {
                return null;
            }

            int index = listResults.SelectedIndices[0];
            return index >= 0 && index < _results.Count ? _results[index] : null;
        }

        private IEnumerable<SearchResult> SelectedResults()
        {
            foreach (int index in listResults.SelectedIndices)
            {
                if (index >= 0 && index < _results.Count)
                {
                    yield return _results[index];
                }
            }
        }

        private void OnListKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                for (int i = 0; i < _results.Count; i++)
                {
                    listResults.SelectedIndices.Add(i);
                }
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                CopySelected(r => r.FullPath);
                e.Handled = true;
            }
        }

        private void OpenSelected()
        {
            if (SelectedResult() is { } result)
            {
                Launch(result.FullPath);
            }
        }

        private void Launch(string path)
        {
            try
            {
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex) when (ex is Win32Exception or IOException or InvalidOperationException)
            {
                MessageBox.Show(this, Localize("\"{0}\" could not be opened: {1}", path, ex.Message), "FinderSeeker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ShowSelectedInExplorer()
        {
            if (SelectedResult() is { } result)
            {
                try
                {
                    Process.Start(new ProcessStartInfo("explorer.exe", $"/select,\"{result.FullPath}\"") { UseShellExecute = true });
                }
                catch (Exception ex) when (ex is Win32Exception or InvalidOperationException)
                {
                    Log.Error("Unable to open Explorer.", ex);
                }
            }
        }

        private void CopySelected(Func<SearchResult, string> text)
        {
            var lines = SelectedResults().Select(text).ToList();
            if (lines.Count > 0)
            {
                try
                {
                    Clipboard.SetText(string.Join(Environment.NewLine, lines));
                }
                catch (ExternalException ex)
                {
                    Log.Warning("The clipboard was busy.", ex);
                }
            }
        }

        #endregion

        #region Commands.

        private void Browse()
        {
            using var dialog = new FolderBrowserDialog { SelectedPath = comboLocation.Text.Split(';')[0].Trim() };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                comboLocation.Text = dialog.SelectedPath;
            }
        }

        private void SetPreviewVisible(bool visible)
        {
            if (visible && splitMain.Panel2Collapsed)
            {
                splitMain.Panel2Collapsed = false;
                if (_loaded)
                {
                    SetPreviewWidth();
                }
                previewPanel.ShowResult(SelectedResult());
            }
            else if (!visible && !splitMain.Panel2Collapsed)
            {
                _settings.PreviewWidth = splitMain.Panel2.Width;
                splitMain.Panel2Collapsed = true;
            }
        }

        private void SetPreviewWidth()
        {
            splitMain.SplitterDistance = Math.Max(100, splitMain.Width - Math.Max(200, _settings.PreviewWidth) - splitMain.SplitterWidth);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _loaded = true;
            if (!splitMain.Panel2Collapsed)
            {
                SetPreviewWidth();
            }
        }

        private static void OpenLogFolder()
        {
            Directory.CreateDirectory(Log.Directory);
            Process.Start(new ProcessStartInfo(Log.Directory) { UseShellExecute = true });
        }

        private void ShowAbout()
        {
            var version = typeof(FormMain).Assembly.GetName().Version?.ToString(3);
            MessageBox.Show(this,
                $"FinderSeeker {version}\r\n\r\n" + Localize("A fast file and content search for Windows.") + "\r\n\r\nCopyright © NetworkDLS",
                Localize("About FinderSeeker"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && _state == State.Running)
            {
                ToggleSearch();
                return true;
            }
            if (keyData == (Keys.Control | Keys.F))
            {
                comboName.Focus();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.F))
            {
                comboContent.Focus();
                return true;
            }
            if (keyData == Keys.F5)
            {
                if (_state != State.Running)
                {
                    StartSearch();
                }
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _session?.Cancel();
            CaptureSettings();
            _settings.Save();
            base.OnFormClosing(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _icons.Dispose();
        }

        #endregion
    }
}
