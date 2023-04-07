using FinderSeeker.Properties;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FinderSeeker
{
    public partial class FormMain : Form
    {
        #region Private types.

        private class SearchState
        {
            public ulong ProcessedSize { get; set; }
            public int FileCount { get; set; }
            public int DirCount { get; set; }
            public int FoundCount { get; set; }
            public List<FileInfo> FoundItems { get; set; } = new();
            public List<string> FileExtensions { get; set; } = new();
            public List<string> FileNames { get; set; } = new();
        }

        private class SearchOptions
        {
            public bool OptionsLoaded { get; set; } = false;
            public List<string> SearchPatterns { get; set; } = new List<string>();
            public string SearchLocation { get; set; } = Path.GetPathRoot(Environment.SystemDirectory) ?? string.Empty;
            public string ContainsText { get; set; } = string.Empty;
            public bool PatternCaseSensitive { get; set; } = false;
            public bool ContainsTextCaseSensitive { get; set; } = false;
            public bool UseModifiedByBefore { get; set; } = false;
            public bool UseModifiedByAfter { get; set; } = false;
            public DateTime ModifiedByBefore { get; set; } = DateTime.Now;
            public DateTime ModifiedByAfter { get; set; } = DateTime.Now;
            public bool UseSizeGreater { get; set; } = false;
            public bool UseSizeLess { get; set; } = false;
            public int SizeGreater { get; set; } = 0;
            public int SizeLess { get; set; } = 0;
            public int SizeGreaterRaw { get; set; } = 0;
            public int SizeLessRaw { get; set; } = 0;
            public string SizeIdentifier { get; set; } = "Size (KB)";
        }

        #endregion

        private bool isRunning = false;
        private bool keepRunning = false;
        private SearchOptions searchOptions = new();
        private SearchState searchState = new();
        private readonly System.Windows.Forms.Timer timer = new();
        private bool patternCaseSensitive = false;
        private bool containsTextCaseSensitive = false;
        private readonly ListViewColumnSorterExt fileSorter;
        private readonly ImageList imageList = new();

        public FormMain()
        {
            InitializeComponent();

            this.Resize += FormMain_Resize;

            listViewResults.DoubleBuffering(true); //after the InitializeComponent();
            listViewResults.ColumnClick += ListViewResults_ColumnClick;


            imageList = new ImageList()
            {
                ColorDepth = ColorDepth.Depth32Bit
            };

            listViewResults.SmallImageList = imageList;
            listViewResults.LargeImageList = imageList;

            splitContainerVert.SizeChanged += SplitContainerVert_SizeChanged;
            splitContainerVert.SplitterMoving += SplitContainerVert_SplitterMoving;
            splitContainerVert.SplitterMoved += SplitContainerVert_SplitterMoved;

            fileSorter = new ListViewColumnSorterExt(listViewResults);

            timer.Tick += Timer_Tick;
            timer.Interval = 100;
            timer.Start();

            LoadSearchSettings();

            checkBoxSizeGreater_CheckedChanged(this, new EventArgs());
            checkBoxSizeLess_CheckedChanged(this, new EventArgs());
            checkBoxModifiedByBefore_CheckedChanged(this, new EventArgs());
            checkBoxModifiedByAfter_CheckedChanged(this, new EventArgs());

            if (patternCaseSensitive)
                buttonPatternCaseSensitive.Image = Resources.CaseSensitiveSelected;
            else buttonPatternCaseSensitive.Image = Resources.CaseSensitiveNotSelected;

            if (containsTextCaseSensitive)
                buttonContainsTextCaseSensitive.Image = Resources.CaseSensitiveSelected;
            else buttonContainsTextCaseSensitive.Image = Resources.CaseSensitiveNotSelected;
            LoadOriginalSizes();

            this.Width -= splitContainerVert.Panel2.Width;
            splitContainerVert.Panel2Collapsed = true;
            SplitContainerVert_SizeChanged(this, new EventArgs());
        }

        private void SplitContainerVert_SplitterMoved(object? sender, SplitterEventArgs e)
        {
            SplitContainerVert_SizeChanged(sender, new EventArgs());
        }

        private void SplitContainerVert_SplitterMoving(object? sender, SplitterCancelEventArgs e)
        {
            SplitContainerVert_SizeChanged(sender, new EventArgs());
        }

        private static class Sizes
        {
            public static int ComboBoxPatternWidth { get; set; }
            public static int SplitContainerVertPanel1Width { get; set; }
            public static int SplitContainerVertPanel1Height { get; set; }
            public static int ComboBoxContainsTextWidth { get; set; }
            public static int GroupBoxCriteraWidth { get; set; }
            public static int ListViewResultsWidth { get; set; }
            public static int ListViewResultsHeight { get; set; }
        }

        private void LoadOriginalSizes()
        {
            Sizes.ComboBoxPatternWidth = comboBoxPattern.Width;
            Sizes.SplitContainerVertPanel1Width = splitContainerVert.Panel1.Width;
            Sizes.SplitContainerVertPanel1Height = splitContainerVert.Panel1.Height;
            Sizes.GroupBoxCriteraWidth = groupBoxCritera.Width;
            Sizes.ListViewResultsWidth = listViewResults.Width;
            Sizes.ListViewResultsHeight = listViewResults.Height;
        }

        private void SplitContainerVert_SizeChanged(object? sender, EventArgs e)
        {
            listViewResults.Width = Sizes.ListViewResultsWidth - (Sizes.SplitContainerVertPanel1Width - splitContainerVert.Panel1.Width);
            listViewResults.Height = Sizes.ListViewResultsHeight - (Sizes.SplitContainerVertPanel1Height - splitContainerVert.Panel1.Height);

            groupBoxCritera.Width = Sizes.GroupBoxCriteraWidth - (Sizes.SplitContainerVertPanel1Width - splitContainerVert.Panel1.Width);
            comboBoxPattern.Width = Sizes.ComboBoxPatternWidth - (Sizes.SplitContainerVertPanel1Width - splitContainerVert.Panel1.Width);
            buttonPatternCaseSensitive.Left = comboBoxPattern.Left + comboBoxPattern.Width + 6;
            comboBoxContainsText.Width = comboBoxPattern.Width;
            buttonContainsTextCaseSensitive.Left = buttonPatternCaseSensitive.Left;
            comboBoxLocation.Width = comboBoxPattern.Width;
            buttonBrowse.Left = buttonPatternCaseSensitive.Left;

            groupBoxResults.Left = groupBoxCritera.Left + groupBoxCritera.Width + 20;
            buttonSearch.Left = groupBoxResults.Left;
        }

        private void FormMain_Resize(object? sender, EventArgs e)
        {

        }

        private void SaveSearchSettings()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var searchOptionsText = JsonConvert.SerializeObject(searchOptions);
            var safePath = Path.Combine(appData, "userOptions.json");
            File.WriteAllText(safePath, searchOptionsText);
        }

        private void LoadSearchSettings()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var safePath = Path.Combine(appData, "userOptions.json");
            var searchOptionsText = File.ReadAllText(safePath);

            var options = JsonConvert.DeserializeObject<SearchOptions>(searchOptionsText) ?? new SearchOptions();

            comboBoxLocation.Text = options.SearchLocation;
            comboBoxPattern.Text = String.Join(";", options.SearchPatterns);
            comboBoxSize.Text = options.SizeIdentifier;

            comboBoxContainsText.Text = options.ContainsText;

            patternCaseSensitive = options.PatternCaseSensitive;
            containsTextCaseSensitive = options.ContainsTextCaseSensitive;

            checkBoxModifiedByBefore.Checked = options.UseModifiedByBefore;
            dateTimePickerModifiedBeforeDate.Value = options.ModifiedByBefore;
            dateTimePickerModifiedBeforeTime.Value = options.ModifiedByBefore;

            checkBoxModifiedByAfter.Checked = options.UseModifiedByAfter;
            dateTimePickerModifiedAfterDate.Value = options.ModifiedByAfter;
            dateTimePickerModifiedAfterTime.Value = options.ModifiedByAfter;

            textBoxSizeGreater.Text = options.SizeGreaterRaw.ToString();
            textBoxSizeLess.Text = options.SizeLessRaw.ToString();

            checkBoxSizeGreater.Checked = options.UseSizeGreater;
            checkBoxSizeLess.Checked = options.UseSizeLess;

            comboBoxSize.Text = options.SizeIdentifier;
        }

        private void LoadSearchOptionsFromForm()
        {
            searchOptions = new SearchOptions();
            searchOptions.SearchLocation = comboBoxLocation.Text.Trim();
            searchOptions.ContainsText = comboBoxContainsText.Text.Trim();
            searchOptions.SearchPatterns = new List<string>(comboBoxPattern.Text.Split(';'));
            searchOptions.SearchPatterns.ForEach(o => o = o.Trim());

            searchOptions.PatternCaseSensitive = patternCaseSensitive;
            searchOptions.ContainsTextCaseSensitive = containsTextCaseSensitive;

            searchOptions.UseModifiedByBefore = checkBoxModifiedByBefore.Checked;
            searchOptions.ModifiedByBefore = dateTimePickerModifiedBeforeDate.Value.Date;
            searchOptions.ModifiedByBefore += dateTimePickerModifiedBeforeDate.Value.TimeOfDay;

            searchOptions.UseModifiedByAfter = checkBoxModifiedByAfter.Checked;
            searchOptions.ModifiedByAfter = dateTimePickerModifiedAfterDate.Value.Date;
            searchOptions.ModifiedByAfter += dateTimePickerModifiedAfterDate.Value.TimeOfDay;

            searchOptions.UseSizeGreater = checkBoxSizeGreater.Checked;
            searchOptions.UseSizeLess = checkBoxSizeLess.Checked;

            searchOptions.SizeIdentifier = comboBoxSize.Text;

            if (int.TryParse(textBoxSizeGreater.Text, out int sizeGreater))
            {
                searchOptions.SizeGreaterRaw = sizeGreater;
                searchOptions.SizeGreater = searchOptions.SizeGreaterRaw;

                if (comboBoxSize.Text == "Size (KB)")
                    searchOptions.SizeGreater *= 1024;
                if (comboBoxSize.Text == "Size (MB)")
                    searchOptions.SizeGreater *= 1048576;
                if (comboBoxSize.Text == "Size (GB)")
                    searchOptions.SizeGreater *= 1073741824;
            }

            if (int.TryParse(textBoxSizeLess.Text, out int sizeLess))
            {
                searchOptions.SizeLessRaw = sizeLess;
                searchOptions.SizeLess = searchOptions.SizeLessRaw;

                if (comboBoxSize.Text == "Size (KB)")
                    searchOptions.SizeLess *= 1024;
                if (comboBoxSize.Text == "Size (MB)")
                    searchOptions.SizeLess *= 1048576;
                if (comboBoxSize.Text == "Size (GB)")
                    searchOptions.SizeLess *= 1073741824;
            }
        }

        private void LoadSearchState()
        {
            searchState = new();

            searchState.FileExtensions = searchOptions.SearchPatterns.Where(o => o.StartsWith("*.")).Select(o => o.TrimStart(new char[] { '*' }).Trim()).ToList();
            searchState.FileNames = searchOptions.SearchPatterns.Where(o => o.StartsWith("*.") == false).Select(o => o.Trim()).ToList();

            if (searchOptions.PatternCaseSensitive == false)
            {
                searchState.FileExtensions = searchState.FileExtensions.ConvertAll(o => o.ToLowerInvariant());
                searchState.FileNames = searchState.FileNames.ConvertAll(o => o.ToLowerInvariant());
            }
        }

        private void ListViewResults_ColumnClick(object? sender, ColumnClickEventArgs e)
        {
            //Add sort images to ListView column header.
            /*
            if (sender is ListView lv)
            {
                if (lv.Columns[e.Column].ImageList.Images.Keys.Contains("Ascending")
                    && lv.Columns[e.Column].ImageList.Images.Keys.Contains("Descending"))
                {
                    switch (Order)
                    {
                        case SortOrder.Ascending:
                            lv.Columns[e.Column].ImageKey = "Ascending";
                            break;
                        case SortOrder.Descending:
                            lv.Columns[e.Column].ImageKey = "Descending";
                            break;
                        case SortOrder.None:
                            lv.Columns[e.Column].ImageKey = string.Empty;
                            break;

                    }
                }
            }
            */
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            labelFoundValue.Text = $"{searchState.FoundCount:n0}";
            labelTotalFilesValue.Text = $"{searchState.FileCount:n0}";
            labelTotalDirsValue.Text = $"{searchState.DirCount:n0}";
            labelProcessedValue.Text = $"{Utility.FormatFileSize(searchState.ProcessedSize, 1)}";

            //Update the list view with the items that have been found.
            lock (searchState.FoundItems)
            {
                searchState.FoundItems.ForEach(o => LogFile(o));
                searchState.FoundItems.Clear();
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                comboBoxLocation.Text = fbd.SelectedPath;
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                keepRunning = false;
            }
            else
            {
                listViewResults.Items.Clear();

                LoadSearchOptionsFromForm();
                SaveSearchSettings();
                LoadSearchState();

                new Thread(ProcessDirectoryThread).Start();

                keepRunning = true;
                isRunning = true;
                UpdateStartButton();
            }
        }

        private void UpdateStartButton()
        {
            if (buttonSearch.InvokeRequired)
            {
                buttonSearch.Invoke(delegate { UpdateStartButton(); });
            }
            else
            {
                if (isRunning)
                {
                    buttonSearch.BackColor = Color.PaleVioletRed;
                    buttonSearch.Text = "Stop!";
                }
                else
                {
                    buttonSearch.BackColor = Color.PaleGreen;
                    buttonSearch.Text = "Search";
                }
            }
        }

        public void ProcessDirectoryThread()
        {
            ProcessDirectory(searchOptions.SearchLocation);
            isRunning = false;
            keepRunning = false;
            UpdateStartButton();
        }

        public void ProcessDirectory(string targetDirectory)
        {
            var directoryInfo = new DirectoryInfo(targetDirectory);

            var fileInfos = directoryInfo.EnumerateFiles("*.*", System.IO.SearchOption.TopDirectoryOnly);

            foreach (var fileInfo in fileInfos)
            {
                if (keepRunning == false)
                {
                    break;
                }
                if (ProcessFile(fileInfo))
                {
                    Debug.WriteLine($"Dir: {targetDirectory}, File: {fileInfo.FullName}");
                }
            }

            var subdirectoryInfos = directoryInfo.EnumerateDirectories("*.*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var subdirectoryInfo in subdirectoryInfos)
            {
                if (keepRunning == false)
                {
                    break;
                }
                ProcessDirectory(subdirectoryInfo.FullName);
            }
        }

        public bool ProcessFile(FileInfo fileInfo)
        {
            searchState.FileCount++;

            bool includeFile = true;
            string casesFileName = fileInfo.Name;

            if (searchOptions.PatternCaseSensitive == false)
            {
                casesFileName = casesFileName.ToLowerInvariant();
            }

            if (includeFile && searchState.FileExtensions.Any())
            {
                if ((includeFile && (searchState.FileExtensions.Contains(Path.GetExtension(casesFileName).ToLowerInvariant()) || searchState.FileExtensions.Contains(".*"))) == false)
                {
                    includeFile = false;
                }
            }

            if (includeFile && searchState.FileNames.Any())
            {
                bool found = false;

                foreach (var fileName in searchState.FileNames)
                {
                    if (casesFileName.Contains(fileName))
                    {
                        found = true;
                        break;
                    }
                }

                includeFile = found;
            }

            if (includeFile && searchOptions.UseModifiedByAfter)
                includeFile = (fileInfo.LastWriteTime >= searchOptions.ModifiedByAfter);
            if (includeFile && searchOptions.UseModifiedByBefore)
                includeFile = (fileInfo.LastWriteTime <= searchOptions.ModifiedByBefore);
            if (includeFile && searchOptions.UseSizeGreater)
                includeFile = (fileInfo.Length >= searchOptions.SizeGreater);
            if (includeFile && searchOptions.UseSizeLess)
                includeFile = (fileInfo.Length <= searchOptions.SizeLess);

            if (includeFile && searchOptions.ContainsText.Length > 0)
            {
                bool foundText = false;
                foreach (var line in File.ReadLines(fileInfo.FullName))
                {
                    if (line.ToLowerInvariant().Contains(searchOptions.ContainsText))
                    {
                        foundText = true;
                        break;
                    }
                    searchState.ProcessedSize += (ulong)line.Length;
                }

                includeFile = foundText;
            }

            if (includeFile)
            {
                lock (searchState.FoundItems)
                {
                    searchState.FoundItems.Add(fileInfo);
                }
                searchState.FoundCount++;
            }

            return includeFile;
        }

        public void LogFile(FileInfo fileInfo)
        {
            //Name, Size, Modified, Created, Location
            string[] row = { fileInfo.Name, $"{fileInfo.Length:N0}", $"{fileInfo.LastWriteTime:yyyy/MM/dd}", $"{fileInfo.CreationTime:yyyy/MM/dd}", Path.GetDirectoryName(fileInfo.FullName) ?? "" };
            var listViewItem = new ListViewItem(row);

            if (listViewResults.InvokeRequired)
            {
                listViewResults.Invoke(delegate { LogFile(fileInfo); });
            }
            else
            {
                var extension = Path.GetExtension(fileInfo.Name.ToLowerInvariant());
                Image? image = null;

                if (imageList.Images.ContainsKey(extension))
                {
                    image = imageList.Images[extension];
                }
                else
                {
                    image = Icon.ExtractAssociatedIcon(fileInfo.FullName)?.ToBitmap();
                    if (image != null)
                    {
                        imageList.Images.Add(extension, image);
                    }
                }

                if (image != null)
                {
                    listViewItem.ImageKey = extension;
                }

                listViewResults.Items.Add(listViewItem);
            }
        }

        #region Toggles.

        private void buttonPatternCaseSensitive_Click(object sender, EventArgs e)
        {
            patternCaseSensitive = !patternCaseSensitive;
            if (patternCaseSensitive)
                buttonPatternCaseSensitive.Image = Resources.CaseSensitiveSelected;
            else buttonPatternCaseSensitive.Image = Resources.CaseSensitiveNotSelected;
        }

        private void buttonContainsTextCaseSensitive_Click(object sender, EventArgs e)
        {
            containsTextCaseSensitive = !containsTextCaseSensitive;
            if (containsTextCaseSensitive)
                buttonContainsTextCaseSensitive.Image = Resources.CaseSensitiveSelected;
            else buttonContainsTextCaseSensitive.Image = Resources.CaseSensitiveNotSelected;
        }

        private void checkBoxSizeGreater_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSizeGreater.Enabled = checkBoxSizeGreater.Checked;
        }

        private void checkBoxSizeLess_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSizeLess.Enabled = checkBoxSizeLess.Checked;
        }

        private void checkBoxModifiedByBefore_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerModifiedBeforeDate.Enabled = checkBoxModifiedByBefore.Checked;
            dateTimePickerModifiedBeforeTime.Enabled = checkBoxModifiedByBefore.Checked;
        }

        private void checkBoxModifiedByAfter_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerModifiedAfterDate.Enabled = checkBoxModifiedByAfter.Checked;
            dateTimePickerModifiedAfterTime.Enabled = checkBoxModifiedByAfter.Checked;
        }

        #endregion
    }
}
