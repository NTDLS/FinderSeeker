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
            labelLocation = new Label();
            comboLocation = new ComboBox();
            buttonBrowse = new Button();
            checkSubfolders = new CheckBox();
            labelName = new Label();
            comboName = new ComboBox();
            comboNameMode = new ComboBox();
            checkNameCase = new CheckBox();
            labelContains = new Label();
            comboContent = new ComboBox();
            comboContentMode = new ComboBox();
            checkContentCase = new CheckBox();
            labelType = new Label();
            comboCategory = new ComboBox();
            labelShow = new Label();
            comboKinds = new ComboBox();
            labelExclude = new Label();
            textExclude = new TextBox();
            checkFullPath = new CheckBox();
            checkHidden = new CheckBox();
            checkSystem = new CheckBox();
            checkBinary = new CheckBox();
            labelMaxContent = new Label();
            numMaxContentMb = new NumericUpDown();
            labelSize = new Label();
            checkUseMin = new CheckBox();
            numMin = new NumericUpDown();
            comboMinUnit = new ComboBox();
            checkUseMax = new CheckBox();
            numMax = new NumericUpDown();
            comboMaxUnit = new ComboBox();
            labelModifiedAfter = new Label();
            dateModifiedAfter = new DateTimePicker();
            labelModifiedBefore = new Label();
            dateModifiedBefore = new DateTimePicker();
            labelCreatedAfter = new Label();
            dateCreatedAfter = new DateTimePicker();
            labelCreatedBefore = new Label();
            dateCreatedBefore = new DateTimePicker();
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
            menuStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            panelCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxContentMb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            contextMenuResults.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { menuFile, menuView, menuHelp });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1180, 24);
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
            statusStrip.Size = new Size(1180, 22);
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
            labelStats.Size = new Size(1165, 17);
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
            panelCriteria.Controls.Add(labelLocation);
            panelCriteria.Controls.Add(comboLocation);
            panelCriteria.Controls.Add(buttonBrowse);
            panelCriteria.Controls.Add(checkSubfolders);
            panelCriteria.Controls.Add(labelName);
            panelCriteria.Controls.Add(comboName);
            panelCriteria.Controls.Add(comboNameMode);
            panelCriteria.Controls.Add(checkNameCase);
            panelCriteria.Controls.Add(labelContains);
            panelCriteria.Controls.Add(comboContent);
            panelCriteria.Controls.Add(comboContentMode);
            panelCriteria.Controls.Add(checkContentCase);
            panelCriteria.Controls.Add(labelType);
            panelCriteria.Controls.Add(comboCategory);
            panelCriteria.Controls.Add(labelShow);
            panelCriteria.Controls.Add(comboKinds);
            panelCriteria.Controls.Add(labelExclude);
            panelCriteria.Controls.Add(textExclude);
            panelCriteria.Controls.Add(checkFullPath);
            panelCriteria.Controls.Add(checkHidden);
            panelCriteria.Controls.Add(checkSystem);
            panelCriteria.Controls.Add(checkBinary);
            panelCriteria.Controls.Add(labelMaxContent);
            panelCriteria.Controls.Add(numMaxContentMb);
            panelCriteria.Controls.Add(labelSize);
            panelCriteria.Controls.Add(checkUseMin);
            panelCriteria.Controls.Add(numMin);
            panelCriteria.Controls.Add(comboMinUnit);
            panelCriteria.Controls.Add(checkUseMax);
            panelCriteria.Controls.Add(numMax);
            panelCriteria.Controls.Add(comboMaxUnit);
            panelCriteria.Controls.Add(labelModifiedAfter);
            panelCriteria.Controls.Add(dateModifiedAfter);
            panelCriteria.Controls.Add(labelModifiedBefore);
            panelCriteria.Controls.Add(dateModifiedBefore);
            panelCriteria.Controls.Add(labelCreatedAfter);
            panelCriteria.Controls.Add(dateCreatedAfter);
            panelCriteria.Controls.Add(labelCreatedBefore);
            panelCriteria.Controls.Add(dateCreatedBefore);
            panelCriteria.Controls.Add(buttonSearch);
            panelCriteria.Dock = DockStyle.Top;
            panelCriteria.Location = new Point(0, 24);
            panelCriteria.Name = "panelCriteria";
            panelCriteria.Size = new Size(1180, 282);
            panelCriteria.TabIndex = 1;
            // 
            // labelLocation
            // 
            labelLocation.AutoSize = true;
            labelLocation.Location = new Point(12, 17);
            labelLocation.Name = "labelLocation";
            labelLocation.Size = new Size(49, 15);
            labelLocation.TabIndex = 0;
            labelLocation.Text = "Look in:";
            // 
            // comboLocation
            // 
            comboLocation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboLocation.FormattingEnabled = true;
            comboLocation.Location = new Point(92, 13);
            comboLocation.Name = "comboLocation";
            comboLocation.Size = new Size(800, 23);
            comboLocation.TabIndex = 1;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonBrowse.Location = new Point(904, 12);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(100, 26);
            buttonBrowse.TabIndex = 2;
            buttonBrowse.Text = "Browse...";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += buttonBrowse_Click;
            // 
            // checkSubfolders
            // 
            checkSubfolders.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkSubfolders.AutoSize = true;
            checkSubfolders.Checked = true;
            checkSubfolders.CheckState = CheckState.Checked;
            checkSubfolders.Location = new Point(1014, 16);
            checkSubfolders.Name = "checkSubfolders";
            checkSubfolders.Size = new Size(123, 19);
            checkSubfolders.TabIndex = 3;
            checkSubfolders.Text = "Include subfolders";
            checkSubfolders.UseVisualStyleBackColor = true;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(12, 47);
            labelName.Name = "labelName";
            labelName.Size = new Size(42, 15);
            labelName.TabIndex = 4;
            labelName.Text = "Name:";
            // 
            // comboName
            // 
            comboName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboName.FormattingEnabled = true;
            comboName.Location = new Point(92, 43);
            comboName.Name = "comboName";
            comboName.Size = new Size(800, 23);
            comboName.TabIndex = 5;
            // 
            // comboNameMode
            // 
            comboNameMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboNameMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboNameMode.FormattingEnabled = true;
            comboNameMode.Location = new Point(904, 43);
            comboNameMode.Name = "comboNameMode";
            comboNameMode.Size = new Size(150, 23);
            comboNameMode.TabIndex = 6;
            // 
            // checkNameCase
            // 
            checkNameCase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkNameCase.Appearance = Appearance.Button;
            checkNameCase.Location = new Point(1062, 42);
            checkNameCase.Name = "checkNameCase";
            checkNameCase.Size = new Size(36, 26);
            checkNameCase.TabIndex = 7;
            checkNameCase.TextAlign = ContentAlignment.MiddleCenter;
            checkNameCase.UseVisualStyleBackColor = true;
            // 
            // labelContains
            // 
            labelContains.AutoSize = true;
            labelContains.Location = new Point(12, 77);
            labelContains.Name = "labelContains";
            labelContains.Size = new Size(57, 15);
            labelContains.TabIndex = 8;
            labelContains.Text = "Contains:";
            // 
            // comboContent
            // 
            comboContent.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboContent.FormattingEnabled = true;
            comboContent.Location = new Point(92, 73);
            comboContent.Name = "comboContent";
            comboContent.Size = new Size(800, 23);
            comboContent.TabIndex = 9;
            // 
            // comboContentMode
            // 
            comboContentMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            comboContentMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboContentMode.FormattingEnabled = true;
            comboContentMode.Location = new Point(904, 73);
            comboContentMode.Name = "comboContentMode";
            comboContentMode.Size = new Size(150, 23);
            comboContentMode.TabIndex = 10;
            // 
            // checkContentCase
            // 
            checkContentCase.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkContentCase.Appearance = Appearance.Button;
            checkContentCase.Location = new Point(1062, 72);
            checkContentCase.Name = "checkContentCase";
            checkContentCase.Size = new Size(36, 26);
            checkContentCase.TabIndex = 11;
            checkContentCase.TextAlign = ContentAlignment.MiddleCenter;
            checkContentCase.UseVisualStyleBackColor = true;
            // 
            // labelType
            // 
            labelType.AutoSize = true;
            labelType.Location = new Point(12, 111);
            labelType.Name = "labelType";
            labelType.Size = new Size(35, 15);
            labelType.TabIndex = 12;
            labelType.Text = "Type:";
            // 
            // comboCategory
            // 
            comboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCategory.FormattingEnabled = true;
            comboCategory.Location = new Point(92, 107);
            comboCategory.Name = "comboCategory";
            comboCategory.Size = new Size(150, 23);
            comboCategory.TabIndex = 13;
            // 
            // labelShow
            // 
            labelShow.AutoSize = true;
            labelShow.Location = new Point(262, 111);
            labelShow.Name = "labelShow";
            labelShow.Size = new Size(39, 15);
            labelShow.TabIndex = 14;
            labelShow.Text = "Show:";
            // 
            // comboKinds
            // 
            comboKinds.DropDownStyle = ComboBoxStyle.DropDownList;
            comboKinds.FormattingEnabled = true;
            comboKinds.Location = new Point(310, 107);
            comboKinds.Name = "comboKinds";
            comboKinds.Size = new Size(150, 23);
            comboKinds.TabIndex = 15;
            // 
            // labelExclude
            // 
            labelExclude.AutoSize = true;
            labelExclude.Location = new Point(482, 111);
            labelExclude.Name = "labelExclude";
            labelExclude.Size = new Size(50, 15);
            labelExclude.TabIndex = 16;
            labelExclude.Text = "Exclude:";
            // 
            // textExclude
            // 
            textExclude.Location = new Point(542, 107);
            textExclude.Name = "textExclude";
            textExclude.Size = new Size(200, 23);
            textExclude.TabIndex = 17;
            // 
            // checkFullPath
            // 
            checkFullPath.AutoSize = true;
            checkFullPath.Location = new Point(756, 109);
            checkFullPath.Name = "checkFullPath";
            checkFullPath.Size = new Size(127, 19);
            checkFullPath.TabIndex = 18;
            checkFullPath.Text = "Match the full path";
            checkFullPath.UseVisualStyleBackColor = true;
            // 
            // checkHidden
            // 
            checkHidden.AutoSize = true;
            checkHidden.Checked = true;
            checkHidden.CheckState = CheckState.Checked;
            checkHidden.Location = new Point(900, 109);
            checkHidden.Name = "checkHidden";
            checkHidden.Size = new Size(97, 19);
            checkHidden.TabIndex = 19;
            checkHidden.Text = "Hidden items";
            checkHidden.UseVisualStyleBackColor = true;
            // 
            // checkSystem
            // 
            checkSystem.AutoSize = true;
            checkSystem.Location = new Point(1006, 109);
            checkSystem.Name = "checkSystem";
            checkSystem.Size = new Size(96, 19);
            checkSystem.TabIndex = 20;
            checkSystem.Text = "System items";
            checkSystem.UseVisualStyleBackColor = true;
            // 
            // checkBinary
            // 
            checkBinary.AutoSize = true;
            checkBinary.Location = new Point(12, 143);
            checkBinary.Name = "checkBinary";
            checkBinary.Size = new Size(121, 19);
            checkBinary.TabIndex = 21;
            checkBinary.Text = "Search binary files";
            checkBinary.UseVisualStyleBackColor = true;
            // 
            // labelMaxContent
            // 
            labelMaxContent.AutoSize = true;
            labelMaxContent.Location = new Point(200, 144);
            labelMaxContent.Name = "labelMaxContent";
            labelMaxContent.Size = new Size(167, 15);
            labelMaxContent.TabIndex = 22;
            labelMaxContent.Text = "Skip text files larger than (MB):";
            // 
            // numMaxContentMb
            // 
            numMaxContentMb.Location = new Point(400, 140);
            numMaxContentMb.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numMaxContentMb.Name = "numMaxContentMb";
            numMaxContentMb.Size = new Size(80, 23);
            numMaxContentMb.TabIndex = 23;
            numMaxContentMb.ThousandsSeparator = true;
            // 
            // labelSize
            // 
            labelSize.AutoSize = true;
            labelSize.Location = new Point(12, 177);
            labelSize.Name = "labelSize";
            labelSize.Size = new Size(30, 15);
            labelSize.TabIndex = 24;
            labelSize.Text = "Size:";
            // 
            // checkUseMin
            // 
            checkUseMin.AutoSize = true;
            checkUseMin.Location = new Point(92, 176);
            checkUseMin.Name = "checkUseMin";
            checkUseMin.Size = new Size(65, 19);
            checkUseMin.TabIndex = 25;
            checkUseMin.Text = "At least";
            checkUseMin.UseVisualStyleBackColor = true;
            checkUseMin.CheckedChanged += checkUseSize_CheckedChanged;
            // 
            // numMin
            // 
            numMin.Location = new Point(170, 173);
            numMin.Maximum = new decimal(new int[] { -727379968, 232, 0, 0 });
            numMin.Name = "numMin";
            numMin.Size = new Size(110, 23);
            numMin.TabIndex = 26;
            numMin.ThousandsSeparator = true;
            // 
            // comboMinUnit
            // 
            comboMinUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            comboMinUnit.FormattingEnabled = true;
            comboMinUnit.Location = new Point(288, 173);
            comboMinUnit.Name = "comboMinUnit";
            comboMinUnit.Size = new Size(72, 23);
            comboMinUnit.TabIndex = 27;
            // 
            // checkUseMax
            // 
            checkUseMax.AutoSize = true;
            checkUseMax.Location = new Point(392, 176);
            checkUseMax.Name = "checkUseMax";
            checkUseMax.Size = new Size(68, 19);
            checkUseMax.TabIndex = 28;
            checkUseMax.Text = "At most";
            checkUseMax.UseVisualStyleBackColor = true;
            checkUseMax.CheckedChanged += checkUseSize_CheckedChanged;
            // 
            // numMax
            // 
            numMax.Location = new Point(470, 173);
            numMax.Maximum = new decimal(new int[] { -727379968, 232, 0, 0 });
            numMax.Name = "numMax";
            numMax.Size = new Size(110, 23);
            numMax.TabIndex = 29;
            numMax.ThousandsSeparator = true;
            // 
            // comboMaxUnit
            // 
            comboMaxUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            comboMaxUnit.FormattingEnabled = true;
            comboMaxUnit.Location = new Point(588, 173);
            comboMaxUnit.Name = "comboMaxUnit";
            comboMaxUnit.Size = new Size(72, 23);
            comboMaxUnit.TabIndex = 30;
            // 
            // labelModifiedAfter
            // 
            labelModifiedAfter.AutoSize = true;
            labelModifiedAfter.Location = new Point(12, 211);
            labelModifiedAfter.Name = "labelModifiedAfter";
            labelModifiedAfter.Size = new Size(85, 15);
            labelModifiedAfter.TabIndex = 31;
            labelModifiedAfter.Text = "Modified after:";
            // 
            // dateModifiedAfter
            // 
            dateModifiedAfter.Checked = false;
            dateModifiedAfter.CustomFormat = "yyyy-MM-dd HH:mm";
            dateModifiedAfter.Format = DateTimePickerFormat.Custom;
            dateModifiedAfter.Location = new Point(112, 207);
            dateModifiedAfter.Name = "dateModifiedAfter";
            dateModifiedAfter.ShowCheckBox = true;
            dateModifiedAfter.Size = new Size(170, 23);
            dateModifiedAfter.TabIndex = 32;
            // 
            // labelModifiedBefore
            // 
            labelModifiedBefore.AutoSize = true;
            labelModifiedBefore.Location = new Point(296, 211);
            labelModifiedBefore.Name = "labelModifiedBefore";
            labelModifiedBefore.Size = new Size(44, 15);
            labelModifiedBefore.TabIndex = 33;
            labelModifiedBefore.Text = "before:";
            // 
            // dateModifiedBefore
            // 
            dateModifiedBefore.Checked = false;
            dateModifiedBefore.CustomFormat = "yyyy-MM-dd HH:mm";
            dateModifiedBefore.Format = DateTimePickerFormat.Custom;
            dateModifiedBefore.Location = new Point(348, 207);
            dateModifiedBefore.Name = "dateModifiedBefore";
            dateModifiedBefore.ShowCheckBox = true;
            dateModifiedBefore.Size = new Size(170, 23);
            dateModifiedBefore.TabIndex = 34;
            // 
            // labelCreatedAfter
            // 
            labelCreatedAfter.AutoSize = true;
            labelCreatedAfter.Location = new Point(546, 211);
            labelCreatedAfter.Name = "labelCreatedAfter";
            labelCreatedAfter.Size = new Size(78, 15);
            labelCreatedAfter.TabIndex = 35;
            labelCreatedAfter.Text = "Created after:";
            // 
            // dateCreatedAfter
            // 
            dateCreatedAfter.Checked = false;
            dateCreatedAfter.CustomFormat = "yyyy-MM-dd HH:mm";
            dateCreatedAfter.Format = DateTimePickerFormat.Custom;
            dateCreatedAfter.Location = new Point(640, 207);
            dateCreatedAfter.Name = "dateCreatedAfter";
            dateCreatedAfter.ShowCheckBox = true;
            dateCreatedAfter.Size = new Size(170, 23);
            dateCreatedAfter.TabIndex = 36;
            // 
            // labelCreatedBefore
            // 
            labelCreatedBefore.AutoSize = true;
            labelCreatedBefore.Location = new Point(824, 211);
            labelCreatedBefore.Name = "labelCreatedBefore";
            labelCreatedBefore.Size = new Size(44, 15);
            labelCreatedBefore.TabIndex = 37;
            labelCreatedBefore.Text = "before:";
            // 
            // dateCreatedBefore
            // 
            dateCreatedBefore.Checked = false;
            dateCreatedBefore.CustomFormat = "yyyy-MM-dd HH:mm";
            dateCreatedBefore.Format = DateTimePickerFormat.Custom;
            dateCreatedBefore.Location = new Point(876, 207);
            dateCreatedBefore.Name = "dateCreatedBefore";
            dateCreatedBefore.ShowCheckBox = true;
            dateCreatedBefore.Size = new Size(170, 23);
            dateCreatedBefore.TabIndex = 38;
            // 
            // buttonSearch
            // 
            buttonSearch.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            buttonSearch.Location = new Point(12, 242);
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
            splitMain.Location = new Point(0, 306);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(listResults);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(previewPanel);
            splitMain.Size = new Size(1180, 392);
            splitMain.SplitterDistance = 750;
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
            listResults.Size = new Size(750, 392);
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
            previewPanel.Location = new Point(0, 0);
            previewPanel.Name = "previewPanel";
            previewPanel.Size = new Size(426, 392);
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
            // FormMain
            // 
            AcceptButton = buttonSearch;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1180, 720);
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
            panelCriteria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMaxContentMb).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMax).EndInit();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            contextMenuResults.ResumeLayout(false);
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
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox comboCategory;
        private System.Windows.Forms.Label labelShow;
        private System.Windows.Forms.ComboBox comboKinds;
        private System.Windows.Forms.Label labelExclude;
        private System.Windows.Forms.TextBox textExclude;
        private System.Windows.Forms.CheckBox checkFullPath;
        private System.Windows.Forms.CheckBox checkHidden;
        private System.Windows.Forms.CheckBox checkSystem;
        private System.Windows.Forms.CheckBox checkBinary;
        private System.Windows.Forms.Label labelMaxContent;
        private System.Windows.Forms.NumericUpDown numMaxContentMb;
        private System.Windows.Forms.Label labelSize;
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
    }
}
