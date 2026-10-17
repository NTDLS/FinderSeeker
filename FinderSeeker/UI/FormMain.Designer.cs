namespace FinderSeeker.UI
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip = new MenuStrip();
            menuFile = new ToolStripMenuItem();
            menuLanguage = new ToolStripMenuItem();
            menuFileSeparator = new ToolStripSeparator();
            menuExit = new ToolStripMenuItem();
            menuView = new ToolStripMenuItem();
            menuShowPreview = new ToolStripMenuItem();
            menuHelp = new ToolStripMenuItem();
            menuOpenLog = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            statusStrip = new StatusStrip();
            labelState = new ToolStripStatusLabel();
            progressSearch = new ToolStripProgressBar();
            labelStats = new ToolStripStatusLabel();
            labelTime = new ToolStripStatusLabel();
            panelCriteria = new Panel();
            splitContainerTabs = new SplitContainer();
            tabControl1 = new TabControl();
            tabPageBasic = new TabPage();
            labelLocation = new Label();
            checkSystem = new CheckBox();
            checkHidden = new CheckBox();
            checkContentCase = new CheckBox();
            comboLocation = new ComboBox();
            comboContent = new ComboBox();
            comboContentMode = new ComboBox();
            checkNameCase = new CheckBox();
            labelContains = new Label();
            labelName = new Label();
            comboNameMode = new ComboBox();
            buttonBrowse = new Button();
            comboName = new ComboBox();
            checkSubfolders = new CheckBox();
            tabPageSizes = new TabPage();
            checkUseMin = new CheckBox();
            numMin = new NumericUpDown();
            comboMinUnit = new ComboBox();
            comboMaxUnit = new ComboBox();
            checkUseMax = new CheckBox();
            numMax = new NumericUpDown();
            tabPageDates = new TabPage();
            groupBoxModifiedDate = new GroupBox();
            dateModifiedAfter = new DateTimePicker();
            dateModifiedBefore = new DateTimePicker();
            labelModifiedAfter = new Label();
            labelModifiedBefore = new Label();
            groupBoxCreatedDate = new GroupBox();
            dateCreatedAfter = new DateTimePicker();
            dateCreatedBefore = new DateTimePicker();
            labelCreatedAfter = new Label();
            labelCreatedBefore = new Label();
            tabPageOptions = new TabPage();
            numMaxContentMb = new NumericUpDown();
            checkBinary = new CheckBox();
            labelMaxContent = new Label();
            buttonSearch = new Button();
            splitMain = new SplitContainer();
            listResults = new DoubleBufferedListView();
            columnName = new ColumnHeader();
            columnFolder = new ColumnHeader();
            columnSize = new ColumnHeader();
            columnModified = new ColumnHeader();
            columnCreated = new ColumnHeader();
            columnMatch = new ColumnHeader();
            contextMenuResults = new ContextMenuStrip(components);
            menuOpen = new ToolStripMenuItem();
            menuShowInExplorer = new ToolStripMenuItem();
            menuResultsSeparator = new ToolStripSeparator();
            menuCopyPath = new ToolStripMenuItem();
            menuCopyName = new ToolStripMenuItem();
            menuCopyFolder = new ToolStripMenuItem();
            previewPanel = new PreviewPanel();
            timerUi = new System.Windows.Forms.Timer(components);
            timerPreview = new System.Windows.Forms.Timer(components);
            toolTip = new ToolTip(components);
            tabControl2 = new TabControl();
            tabPagePreview = new TabPage();
            tabPageProperties = new TabPage();
            listViewProperties = new ListView();
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            panelCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerTabs).BeginInit();
            splitContainerTabs.Panel1.SuspendLayout();
            splitContainerTabs.Panel2.SuspendLayout();
            splitContainerTabs.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageBasic.SuspendLayout();
            tabPageSizes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMax).BeginInit();
            tabPageDates.SuspendLayout();
            groupBoxModifiedDate.SuspendLayout();
            groupBoxCreatedDate.SuspendLayout();
            tabPageOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxContentMb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            contextMenuResults.SuspendLayout();
            tabControl2.SuspendLayout();
            tabPagePreview.SuspendLayout();
            tabPageProperties.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { menuFile, menuView, menuHelp });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(984, 24);
            menuStrip.TabIndex = 0;
            // 
            // menuFile
            // 
            menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuLanguage, menuFileSeparator, menuExit });
            menuFile.Name = "menuFile";
            menuFile.Size = new Size(37, 20);
            menuFile.Text = "File";
            // 
            // menuLanguage
            // 
            menuLanguage.Name = "menuLanguage";
            menuLanguage.Size = new Size(135, 22);
            menuLanguage.Text = "Language...";
            menuLanguage.Click += menuLanguage_Click;
            // 
            // menuFileSeparator
            // 
            menuFileSeparator.Name = "menuFileSeparator";
            menuFileSeparator.Size = new Size(132, 6);
            // 
            // menuExit
            // 
            menuExit.Name = "menuExit";
            menuExit.Size = new Size(135, 22);
            menuExit.Text = "Exit";
            menuExit.Click += menuExit_Click;
            // 
            // menuView
            // 
            menuView.DropDownItems.AddRange(new ToolStripItem[] { menuShowPreview });
            menuView.Name = "menuView";
            menuView.Size = new Size(44, 20);
            menuView.Text = "View";
            // 
            // menuShowPreview
            // 
            menuShowPreview.CheckOnClick = true;
            menuShowPreview.Name = "menuShowPreview";
            menuShowPreview.ShortcutKeys = Keys.F11;
            menuShowPreview.Size = new Size(172, 22);
            menuShowPreview.Text = "Preview panel";
            menuShowPreview.CheckedChanged += menuShowPreview_CheckedChanged;
            // 
            // menuHelp
            // 
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuOpenLog, menuAbout });
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(44, 20);
            menuHelp.Text = "Help";
            // 
            // menuOpenLog
            // 
            menuOpenLog.Name = "menuOpenLog";
            menuOpenLog.Size = new Size(177, 22);
            menuOpenLog.Text = "Open log folder";
            menuOpenLog.Click += menuOpenLog_Click;
            // 
            // menuAbout
            // 
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(177, 22);
            menuAbout.Text = "About FinderSeeker";
            menuAbout.Click += menuAbout_Click;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { labelState, progressSearch, labelStats, labelTime });
            statusStrip.Location = new Point(0, 698);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(984, 22);
            statusStrip.TabIndex = 3;
            // 
            // labelState
            // 
            labelState.Name = "labelState";
            labelState.Size = new Size(0, 17);
            // 
            // progressSearch
            // 
            progressSearch.MarqueeAnimationSpeed = 30;
            progressSearch.Name = "progressSearch";
            progressSearch.Size = new Size(100, 16);
            progressSearch.Style = ProgressBarStyle.Marquee;
            progressSearch.Visible = false;
            // 
            // labelStats
            // 
            labelStats.Name = "labelStats";
            labelStats.Size = new Size(969, 17);
            labelStats.Spring = true;
            labelStats.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelTime
            // 
            labelTime.Name = "labelTime";
            labelTime.Size = new Size(0, 17);
            // 
            // panelCriteria
            // 
            panelCriteria.Controls.Add(splitContainerTabs);
            panelCriteria.Dock = DockStyle.Top;
            panelCriteria.Location = new Point(0, 24);
            panelCriteria.Name = "panelCriteria";
            panelCriteria.Size = new Size(984, 216);
            panelCriteria.TabIndex = 1;
            // 
            // splitContainerTabs
            // 
            splitContainerTabs.Dock = DockStyle.Fill;
            splitContainerTabs.FixedPanel = FixedPanel.Panel1;
            splitContainerTabs.Location = new Point(0, 0);
            splitContainerTabs.Name = "splitContainerTabs";
            splitContainerTabs.Orientation = Orientation.Horizontal;
            // 
            // splitContainerTabs.Panel1
            // 
            splitContainerTabs.Panel1.Controls.Add(tabControl1);
            // 
            // splitContainerTabs.Panel2
            // 
            splitContainerTabs.Panel2.Controls.Add(buttonSearch);
            splitContainerTabs.Size = new Size(984, 216);
            splitContainerTabs.SplitterDistance = 163;
            splitContainerTabs.TabIndex = 44;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageBasic);
            tabControl1.Controls.Add(tabPageSizes);
            tabControl1.Controls.Add(tabPageDates);
            tabControl1.Controls.Add(tabPageOptions);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(984, 163);
            tabControl1.TabIndex = 43;
            // 
            // tabPageBasic
            // 
            tabPageBasic.Controls.Add(labelLocation);
            tabPageBasic.Controls.Add(checkSystem);
            tabPageBasic.Controls.Add(checkHidden);
            tabPageBasic.Controls.Add(checkContentCase);
            tabPageBasic.Controls.Add(comboLocation);
            tabPageBasic.Controls.Add(comboContent);
            tabPageBasic.Controls.Add(comboContentMode);
            tabPageBasic.Controls.Add(checkNameCase);
            tabPageBasic.Controls.Add(labelContains);
            tabPageBasic.Controls.Add(labelName);
            tabPageBasic.Controls.Add(comboNameMode);
            tabPageBasic.Controls.Add(buttonBrowse);
            tabPageBasic.Controls.Add(comboName);
            tabPageBasic.Controls.Add(checkSubfolders);
            tabPageBasic.Location = new Point(4, 24);
            tabPageBasic.Name = "tabPageBasic";
            tabPageBasic.Padding = new Padding(3);
            tabPageBasic.Size = new Size(976, 135);
            tabPageBasic.TabIndex = 0;
            tabPageBasic.Text = "Basic";
            tabPageBasic.UseVisualStyleBackColor = true;
            // 
            // labelLocation
            // 
            labelLocation.AutoSize = true;
            labelLocation.Location = new Point(6, 9);
            labelLocation.Name = "labelLocation";
            labelLocation.Size = new Size(31, 15);
            labelLocation.TabIndex = 0;
            labelLocation.Text = "Path";
            // 
            // checkSystem
            // 
            checkSystem.AutoSize = true;
            checkSystem.Location = new Point(218, 93);
            checkSystem.Name = "checkSystem";
            checkSystem.Size = new Size(96, 19);
            checkSystem.TabIndex = 20;
            checkSystem.Text = "System items";
            checkSystem.UseVisualStyleBackColor = true;
            // 
            // checkHidden
            // 
            checkHidden.AutoSize = true;
            checkHidden.Checked = true;
            checkHidden.CheckState = CheckState.Checked;
            checkHidden.Location = new Point(76, 93);
            checkHidden.Name = "checkHidden";
            checkHidden.Size = new Size(97, 19);
            checkHidden.TabIndex = 19;
            checkHidden.Text = "Hidden items";
            checkHidden.UseVisualStyleBackColor = true;
            // 
            // checkContentCase
            // 
            checkContentCase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkContentCase.Appearance = Appearance.Button;
            checkContentCase.Location = new Point(932, 61);
            checkContentCase.Name = "checkContentCase";
            checkContentCase.Size = new Size(36, 26);
            checkContentCase.TabIndex = 11;
            checkContentCase.TextAlign = ContentAlignment.MiddleCenter;
            checkContentCase.UseVisualStyleBackColor = true;
            // 
            // comboLocation
            // 
            comboLocation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboLocation.FormattingEnabled = true;
            comboLocation.Location = new Point(76, 6);
            comboLocation.Name = "comboLocation";
            comboLocation.Size = new Size(693, 23);
            comboLocation.TabIndex = 1;
            // 
            // comboContent
            // 
            comboContent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboContent.FormattingEnabled = true;
            comboContent.Location = new Point(76, 64);
            comboContent.Name = "comboContent";
            comboContent.Size = new Size(693, 23);
            comboContent.TabIndex = 9;
            // 
            // comboContentMode
            // 
            comboContentMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboContentMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboContentMode.FormattingEnabled = true;
            comboContentMode.Location = new Point(775, 64);
            comboContentMode.Name = "comboContentMode";
            comboContentMode.Size = new Size(150, 23);
            comboContentMode.TabIndex = 10;
            // 
            // checkNameCase
            // 
            checkNameCase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkNameCase.Appearance = Appearance.Button;
            checkNameCase.Location = new Point(932, 32);
            checkNameCase.Name = "checkNameCase";
            checkNameCase.Size = new Size(36, 26);
            checkNameCase.TabIndex = 7;
            checkNameCase.TextAlign = ContentAlignment.MiddleCenter;
            checkNameCase.UseVisualStyleBackColor = true;
            // 
            // labelContains
            // 
            labelContains.AutoSize = true;
            labelContains.Location = new Point(3, 67);
            labelContains.Name = "labelContains";
            labelContains.Size = new Size(28, 15);
            labelContains.TabIndex = 8;
            labelContains.Text = "Text";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(6, 38);
            labelName.Name = "labelName";
            labelName.Size = new Size(39, 15);
            labelName.TabIndex = 4;
            labelName.Text = "Name";
            // 
            // comboNameMode
            // 
            comboNameMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboNameMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboNameMode.FormattingEnabled = true;
            comboNameMode.Location = new Point(775, 35);
            comboNameMode.Name = "comboNameMode";
            comboNameMode.Size = new Size(150, 23);
            comboNameMode.TabIndex = 6;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonBrowse.Location = new Point(775, 6);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(100, 26);
            buttonBrowse.TabIndex = 2;
            buttonBrowse.Text = "Browse...";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += buttonBrowse_Click;
            // 
            // comboName
            // 
            comboName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboName.FormattingEnabled = true;
            comboName.Location = new Point(76, 35);
            comboName.Name = "comboName";
            comboName.Size = new Size(693, 23);
            comboName.TabIndex = 5;
            // 
            // checkSubfolders
            // 
            checkSubfolders.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkSubfolders.AutoSize = true;
            checkSubfolders.Checked = true;
            checkSubfolders.CheckState = CheckState.Checked;
            checkSubfolders.Location = new Point(881, 10);
            checkSubfolders.Name = "checkSubfolders";
            checkSubfolders.Size = new Size(87, 19);
            checkSubfolders.TabIndex = 3;
            checkSubfolders.Text = "Subfolders?";
            checkSubfolders.UseVisualStyleBackColor = true;
            // 
            // tabPageSizes
            // 
            tabPageSizes.Controls.Add(checkUseMin);
            tabPageSizes.Controls.Add(numMin);
            tabPageSizes.Controls.Add(comboMinUnit);
            tabPageSizes.Controls.Add(comboMaxUnit);
            tabPageSizes.Controls.Add(checkUseMax);
            tabPageSizes.Controls.Add(numMax);
            tabPageSizes.Location = new Point(4, 24);
            tabPageSizes.Name = "tabPageSizes";
            tabPageSizes.Padding = new Padding(3);
            tabPageSizes.Size = new Size(976, 135);
            tabPageSizes.TabIndex = 2;
            tabPageSizes.Text = "Sizes";
            tabPageSizes.UseVisualStyleBackColor = true;
            // 
            // checkUseMin
            // 
            checkUseMin.AutoSize = true;
            checkUseMin.Location = new Point(6, 6);
            checkUseMin.Name = "checkUseMin";
            checkUseMin.Size = new Size(65, 19);
            checkUseMin.TabIndex = 25;
            checkUseMin.Text = "At least";
            checkUseMin.UseVisualStyleBackColor = true;
            checkUseMin.CheckedChanged += checkUseSize_CheckedChanged;
            // 
            // numMin
            // 
            numMin.Location = new Point(113, 6);
            numMin.Maximum = new decimal(new int[] { -727379968, 232, 0, 0 });
            numMin.Name = "numMin";
            numMin.Size = new Size(72, 23);
            numMin.TabIndex = 26;
            numMin.ThousandsSeparator = true;
            // 
            // comboMinUnit
            // 
            comboMinUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            comboMinUnit.FormattingEnabled = true;
            comboMinUnit.Location = new Point(191, 6);
            comboMinUnit.Name = "comboMinUnit";
            comboMinUnit.Size = new Size(72, 23);
            comboMinUnit.TabIndex = 27;
            // 
            // comboMaxUnit
            // 
            comboMaxUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            comboMaxUnit.FormattingEnabled = true;
            comboMaxUnit.Location = new Point(191, 35);
            comboMaxUnit.Name = "comboMaxUnit";
            comboMaxUnit.Size = new Size(72, 23);
            comboMaxUnit.TabIndex = 30;
            // 
            // checkUseMax
            // 
            checkUseMax.AutoSize = true;
            checkUseMax.Location = new Point(6, 36);
            checkUseMax.Name = "checkUseMax";
            checkUseMax.Size = new Size(68, 19);
            checkUseMax.TabIndex = 28;
            checkUseMax.Text = "At most";
            checkUseMax.UseVisualStyleBackColor = true;
            checkUseMax.CheckedChanged += checkUseSize_CheckedChanged;
            // 
            // numMax
            // 
            numMax.Location = new Point(113, 35);
            numMax.Maximum = new decimal(new int[] { -727379968, 232, 0, 0 });
            numMax.Name = "numMax";
            numMax.Size = new Size(72, 23);
            numMax.TabIndex = 29;
            numMax.ThousandsSeparator = true;
            // 
            // tabPageDates
            // 
            tabPageDates.Controls.Add(groupBoxModifiedDate);
            tabPageDates.Controls.Add(groupBoxCreatedDate);
            tabPageDates.Location = new Point(4, 24);
            tabPageDates.Name = "tabPageDates";
            tabPageDates.Padding = new Padding(3);
            tabPageDates.Size = new Size(976, 135);
            tabPageDates.TabIndex = 1;
            tabPageDates.Text = "Dates";
            tabPageDates.UseVisualStyleBackColor = true;
            // 
            // groupBoxModifiedDate
            // 
            groupBoxModifiedDate.Controls.Add(dateModifiedAfter);
            groupBoxModifiedDate.Controls.Add(dateModifiedBefore);
            groupBoxModifiedDate.Controls.Add(labelModifiedAfter);
            groupBoxModifiedDate.Controls.Add(labelModifiedBefore);
            groupBoxModifiedDate.Location = new Point(6, 6);
            groupBoxModifiedDate.Name = "groupBoxModifiedDate";
            groupBoxModifiedDate.Size = new Size(296, 89);
            groupBoxModifiedDate.TabIndex = 41;
            groupBoxModifiedDate.TabStop = false;
            groupBoxModifiedDate.Text = "Modified Date";
            // 
            // dateModifiedAfter
            // 
            dateModifiedAfter.Checked = false;
            dateModifiedAfter.CustomFormat = "yyyy-MM-dd HH:mm";
            dateModifiedAfter.Format = DateTimePickerFormat.Custom;
            dateModifiedAfter.Location = new Point(120, 22);
            dateModifiedAfter.Name = "dateModifiedAfter";
            dateModifiedAfter.ShowCheckBox = true;
            dateModifiedAfter.Size = new Size(170, 23);
            dateModifiedAfter.TabIndex = 32;
            // 
            // dateModifiedBefore
            // 
            dateModifiedBefore.Checked = false;
            dateModifiedBefore.CustomFormat = "yyyy-MM-dd HH:mm";
            dateModifiedBefore.Format = DateTimePickerFormat.Custom;
            dateModifiedBefore.Location = new Point(120, 51);
            dateModifiedBefore.Name = "dateModifiedBefore";
            dateModifiedBefore.ShowCheckBox = true;
            dateModifiedBefore.Size = new Size(170, 23);
            dateModifiedBefore.TabIndex = 34;
            // 
            // labelModifiedAfter
            // 
            labelModifiedAfter.AutoSize = true;
            labelModifiedAfter.Location = new Point(10, 22);
            labelModifiedAfter.Name = "labelModifiedAfter";
            labelModifiedAfter.Size = new Size(33, 15);
            labelModifiedAfter.TabIndex = 31;
            labelModifiedAfter.Text = "After";
            // 
            // labelModifiedBefore
            // 
            labelModifiedBefore.AutoSize = true;
            labelModifiedBefore.Location = new Point(10, 51);
            labelModifiedBefore.Name = "labelModifiedBefore";
            labelModifiedBefore.Size = new Size(41, 15);
            labelModifiedBefore.TabIndex = 33;
            labelModifiedBefore.Text = "Before";
            // 
            // groupBoxCreatedDate
            // 
            groupBoxCreatedDate.Controls.Add(dateCreatedAfter);
            groupBoxCreatedDate.Controls.Add(dateCreatedBefore);
            groupBoxCreatedDate.Controls.Add(labelCreatedAfter);
            groupBoxCreatedDate.Controls.Add(labelCreatedBefore);
            groupBoxCreatedDate.Location = new Point(308, 6);
            groupBoxCreatedDate.Name = "groupBoxCreatedDate";
            groupBoxCreatedDate.Size = new Size(296, 89);
            groupBoxCreatedDate.TabIndex = 42;
            groupBoxCreatedDate.TabStop = false;
            groupBoxCreatedDate.Text = "Created Date";
            // 
            // dateCreatedAfter
            // 
            dateCreatedAfter.Checked = false;
            dateCreatedAfter.CustomFormat = "yyyy-MM-dd HH:mm";
            dateCreatedAfter.Format = DateTimePickerFormat.Custom;
            dateCreatedAfter.Location = new Point(120, 27);
            dateCreatedAfter.Name = "dateCreatedAfter";
            dateCreatedAfter.ShowCheckBox = true;
            dateCreatedAfter.Size = new Size(170, 23);
            dateCreatedAfter.TabIndex = 36;
            // 
            // dateCreatedBefore
            // 
            dateCreatedBefore.Checked = false;
            dateCreatedBefore.CustomFormat = "yyyy-MM-dd HH:mm";
            dateCreatedBefore.Format = DateTimePickerFormat.Custom;
            dateCreatedBefore.Location = new Point(119, 56);
            dateCreatedBefore.Name = "dateCreatedBefore";
            dateCreatedBefore.ShowCheckBox = true;
            dateCreatedBefore.Size = new Size(170, 23);
            dateCreatedBefore.TabIndex = 38;
            // 
            // labelCreatedAfter
            // 
            labelCreatedAfter.AutoSize = true;
            labelCreatedAfter.Location = new Point(6, 27);
            labelCreatedAfter.Name = "labelCreatedAfter";
            labelCreatedAfter.Size = new Size(33, 15);
            labelCreatedAfter.TabIndex = 35;
            labelCreatedAfter.Text = "After";
            // 
            // labelCreatedBefore
            // 
            labelCreatedBefore.AutoSize = true;
            labelCreatedBefore.Location = new Point(6, 58);
            labelCreatedBefore.Name = "labelCreatedBefore";
            labelCreatedBefore.Size = new Size(41, 15);
            labelCreatedBefore.TabIndex = 37;
            labelCreatedBefore.Text = "Before";
            // 
            // tabPageOptions
            // 
            tabPageOptions.Controls.Add(numMaxContentMb);
            tabPageOptions.Controls.Add(checkBinary);
            tabPageOptions.Controls.Add(labelMaxContent);
            tabPageOptions.Location = new Point(4, 24);
            tabPageOptions.Name = "tabPageOptions";
            tabPageOptions.Padding = new Padding(3);
            tabPageOptions.Size = new Size(976, 135);
            tabPageOptions.TabIndex = 3;
            tabPageOptions.Text = "Options";
            tabPageOptions.UseVisualStyleBackColor = true;
            // 
            // numMaxContentMb
            // 
            numMaxContentMb.Location = new Point(6, 61);
            numMaxContentMb.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numMaxContentMb.Name = "numMaxContentMb";
            numMaxContentMb.Size = new Size(80, 23);
            numMaxContentMb.TabIndex = 23;
            numMaxContentMb.ThousandsSeparator = true;
            // 
            // checkBinary
            // 
            checkBinary.AutoSize = true;
            checkBinary.Location = new Point(6, 6);
            checkBinary.Name = "checkBinary";
            checkBinary.Size = new Size(121, 19);
            checkBinary.TabIndex = 21;
            checkBinary.Text = "Search binary files";
            checkBinary.UseVisualStyleBackColor = true;
            // 
            // labelMaxContent
            // 
            labelMaxContent.AutoSize = true;
            labelMaxContent.Location = new Point(6, 43);
            labelMaxContent.Name = "labelMaxContent";
            labelMaxContent.Size = new Size(167, 15);
            labelMaxContent.TabIndex = 22;
            labelMaxContent.Text = "Skip text files larger than (MB):";
            // 
            // buttonSearch
            // 
            buttonSearch.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            buttonSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            buttonSearch.Location = new Point(852, 11);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(120, 32);
            buttonSearch.TabIndex = 39;
            buttonSearch.Text = "Search";
            buttonSearch.UseVisualStyleBackColor = true;
            buttonSearch.Click += buttonSearch_Click;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.FixedPanel = FixedPanel.Panel2;
            splitMain.Location = new Point(0, 240);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(listResults);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(tabControl2);
            splitMain.Size = new Size(984, 458);
            splitMain.SplitterDistance = 554;
            splitMain.TabIndex = 2;
            // 
            // listResults
            // 
            listResults.BorderStyle = BorderStyle.None;
            listResults.Columns.AddRange(new ColumnHeader[] { columnName, columnFolder, columnSize, columnModified, columnCreated, columnMatch });
            listResults.ContextMenuStrip = contextMenuResults;
            listResults.Dock = DockStyle.Fill;
            listResults.FullRowSelect = true;
            listResults.Location = new Point(0, 0);
            listResults.Name = "listResults";
            listResults.Size = new Size(554, 458);
            listResults.TabIndex = 0;
            listResults.UseCompatibleStateImageBehavior = false;
            listResults.View = View.Details;
            listResults.VirtualMode = true;
            listResults.ColumnClick += listResults_ColumnClick;
            listResults.ItemActivate += listResults_ItemActivate;
            listResults.RetrieveVirtualItem += listResults_RetrieveVirtualItem;
            listResults.SelectedIndexChanged += listResults_SelectedIndexChanged;
            listResults.KeyDown += listResults_KeyDown;
            // 
            // columnName
            // 
            columnName.Text = "Name";
            columnName.Width = 240;
            // 
            // columnFolder
            // 
            columnFolder.Text = "Folder";
            columnFolder.Width = 300;
            // 
            // columnSize
            // 
            columnSize.Text = "Size";
            columnSize.TextAlign = HorizontalAlignment.Right;
            columnSize.Width = 90;
            // 
            // columnModified
            // 
            columnModified.Text = "Modified";
            columnModified.Width = 130;
            // 
            // columnCreated
            // 
            columnCreated.Text = "Created";
            columnCreated.Width = 130;
            // 
            // columnMatch
            // 
            columnMatch.Text = "Match";
            columnMatch.Width = 360;
            // 
            // contextMenuResults
            // 
            contextMenuResults.Items.AddRange(new ToolStripItem[] { menuOpen, menuShowInExplorer, menuResultsSeparator, menuCopyPath, menuCopyName, menuCopyFolder });
            contextMenuResults.Name = "contextMenuResults";
            contextMenuResults.Size = new Size(162, 120);
            contextMenuResults.Opening += contextMenuResults_Opening;
            // 
            // menuOpen
            // 
            menuOpen.Name = "menuOpen";
            menuOpen.Size = new Size(161, 22);
            menuOpen.Text = "Open";
            menuOpen.Click += menuOpen_Click;
            // 
            // menuShowInExplorer
            // 
            menuShowInExplorer.Name = "menuShowInExplorer";
            menuShowInExplorer.Size = new Size(161, 22);
            menuShowInExplorer.Text = "Show in Explorer";
            menuShowInExplorer.Click += menuShowInExplorer_Click;
            // 
            // menuResultsSeparator
            // 
            menuResultsSeparator.Name = "menuResultsSeparator";
            menuResultsSeparator.Size = new Size(158, 6);
            // 
            // menuCopyPath
            // 
            menuCopyPath.Name = "menuCopyPath";
            menuCopyPath.Size = new Size(161, 22);
            menuCopyPath.Text = "Copy full path";
            menuCopyPath.Click += menuCopyPath_Click;
            // 
            // menuCopyName
            // 
            menuCopyName.Name = "menuCopyName";
            menuCopyName.Size = new Size(161, 22);
            menuCopyName.Text = "Copy name";
            menuCopyName.Click += menuCopyName_Click;
            // 
            // menuCopyFolder
            // 
            menuCopyFolder.Name = "menuCopyFolder";
            menuCopyFolder.Size = new Size(161, 22);
            menuCopyFolder.Text = "Copy folder";
            menuCopyFolder.Click += menuCopyFolder_Click;
            // 
            // previewPanel
            // 
            previewPanel.Dock = DockStyle.Fill;
            previewPanel.Highlighter = null;
            previewPanel.Location = new Point(3, 3);
            previewPanel.Name = "previewPanel";
            previewPanel.Size = new Size(412, 424);
            previewPanel.TabIndex = 0;
            // 
            // timerUi
            // 
            timerUi.Tick += timerUi_Tick;
            // 
            // timerPreview
            // 
            timerPreview.Interval = 120;
            timerPreview.Tick += timerPreview_Tick;
            // 
            // toolTip
            // 
            toolTip.AutoPopDelay = 12000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 100;
            // 
            // tabControl2
            // 
            tabControl2.Controls.Add(tabPagePreview);
            tabControl2.Controls.Add(tabPageProperties);
            tabControl2.Dock = DockStyle.Fill;
            tabControl2.Location = new Point(0, 0);
            tabControl2.Name = "tabControl2";
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new Size(426, 458);
            tabControl2.TabIndex = 1;
            // 
            // tabPagePreview
            // 
            tabPagePreview.Controls.Add(previewPanel);
            tabPagePreview.Location = new Point(4, 24);
            tabPagePreview.Name = "tabPagePreview";
            tabPagePreview.Padding = new Padding(3);
            tabPagePreview.Size = new Size(418, 430);
            tabPagePreview.TabIndex = 0;
            tabPagePreview.Text = "Preview";
            tabPagePreview.UseVisualStyleBackColor = true;
            // 
            // tabPageProperties
            // 
            tabPageProperties.Controls.Add(listViewProperties);
            tabPageProperties.Location = new Point(4, 24);
            tabPageProperties.Name = "tabPageProperties";
            tabPageProperties.Padding = new Padding(3);
            tabPageProperties.Size = new Size(418, 430);
            tabPageProperties.TabIndex = 1;
            tabPageProperties.Text = "Properties";
            tabPageProperties.UseVisualStyleBackColor = true;
            // 
            // listViewProperties
            // 
            listViewProperties.Dock = DockStyle.Fill;
            listViewProperties.Location = new Point(3, 3);
            listViewProperties.Name = "listViewProperties";
            listViewProperties.Size = new Size(412, 424);
            listViewProperties.TabIndex = 0;
            listViewProperties.UseCompatibleStateImageBehavior = false;
            listViewProperties.View = View.Details;
            // 
            // FormMain
            // 
            AcceptButton = buttonSearch;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 720);
            Controls.Add(splitMain);
            Controls.Add(panelCriteria);
            Controls.Add(menuStrip);
            Controls.Add(statusStrip);
            KeyPreview = true;
            MainMenuStrip = menuStrip;
            MinimumSize = new Size(900, 560);
            Name = "FormMain";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            panelCriteria.ResumeLayout(false);
            splitContainerTabs.Panel1.ResumeLayout(false);
            splitContainerTabs.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerTabs).EndInit();
            splitContainerTabs.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPageBasic.ResumeLayout(false);
            tabPageBasic.PerformLayout();
            tabPageSizes.ResumeLayout(false);
            tabPageSizes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMax).EndInit();
            tabPageDates.ResumeLayout(false);
            groupBoxModifiedDate.ResumeLayout(false);
            groupBoxModifiedDate.PerformLayout();
            groupBoxCreatedDate.ResumeLayout(false);
            groupBoxCreatedDate.PerformLayout();
            tabPageOptions.ResumeLayout(false);
            tabPageOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxContentMb).EndInit();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            contextMenuResults.ResumeLayout(false);
            tabControl2.ResumeLayout(false);
            tabPagePreview.ResumeLayout(false);
            tabPageProperties.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuLanguage;
        private System.Windows.Forms.ToolStripSeparator menuFileSeparator;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuShowPreview;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuOpenLog;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel labelState;
        private System.Windows.Forms.ToolStripProgressBar progressSearch;
        private System.Windows.Forms.ToolStripStatusLabel labelStats;
        private System.Windows.Forms.ToolStripStatusLabel labelTime;
        private System.Windows.Forms.Panel panelCriteria;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.ComboBox comboLocation;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.CheckBox checkSubfolders;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ComboBox comboName;
        private System.Windows.Forms.ComboBox comboNameMode;
        private System.Windows.Forms.CheckBox checkNameCase;
        private System.Windows.Forms.Label labelContains;
        private System.Windows.Forms.ComboBox comboContent;
        private System.Windows.Forms.ComboBox comboContentMode;
        private System.Windows.Forms.CheckBox checkContentCase;
        private System.Windows.Forms.CheckBox checkHidden;
        private System.Windows.Forms.CheckBox checkSystem;
        private System.Windows.Forms.CheckBox checkBinary;
        private System.Windows.Forms.Label labelMaxContent;
        private System.Windows.Forms.NumericUpDown numMaxContentMb;
        private System.Windows.Forms.CheckBox checkUseMin;
        private System.Windows.Forms.NumericUpDown numMin;
        private System.Windows.Forms.ComboBox comboMinUnit;
        private System.Windows.Forms.CheckBox checkUseMax;
        private System.Windows.Forms.NumericUpDown numMax;
        private System.Windows.Forms.ComboBox comboMaxUnit;
        private System.Windows.Forms.Label labelModifiedAfter;
        private System.Windows.Forms.DateTimePicker dateModifiedAfter;
        private System.Windows.Forms.Label labelModifiedBefore;
        private System.Windows.Forms.DateTimePicker dateModifiedBefore;
        private System.Windows.Forms.Label labelCreatedAfter;
        private System.Windows.Forms.DateTimePicker dateCreatedAfter;
        private System.Windows.Forms.Label labelCreatedBefore;
        private System.Windows.Forms.DateTimePicker dateCreatedBefore;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.SplitContainer splitMain;
        private DoubleBufferedListView listResults;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnFolder;
        private System.Windows.Forms.ColumnHeader columnSize;
        private System.Windows.Forms.ColumnHeader columnModified;
        private System.Windows.Forms.ColumnHeader columnCreated;
        private System.Windows.Forms.ColumnHeader columnMatch;
        private System.Windows.Forms.ContextMenuStrip contextMenuResults;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.ToolStripMenuItem menuShowInExplorer;
        private System.Windows.Forms.ToolStripSeparator menuResultsSeparator;
        private System.Windows.Forms.ToolStripMenuItem menuCopyPath;
        private System.Windows.Forms.ToolStripMenuItem menuCopyName;
        private System.Windows.Forms.ToolStripMenuItem menuCopyFolder;
        private PreviewPanel previewPanel;
        private System.Windows.Forms.Timer timerUi;
        private System.Windows.Forms.Timer timerPreview;
        private System.Windows.Forms.ToolTip toolTip;
        private GroupBox groupBoxCreatedDate;
        private GroupBox groupBoxModifiedDate;
        private TabControl tabControl1;
        private TabPage tabPageBasic;
        private TabPage tabPageDates;
        private TabPage tabPageSizes;
        private TabPage tabPageOptions;
        private SplitContainer splitContainerTabs;
        private TabControl tabControl2;
        private TabPage tabPagePreview;
        private TabPage tabPageProperties;
        private ListView listViewProperties;
    }
}
