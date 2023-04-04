namespace FinderSeeker
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.labelSearchPattern = new System.Windows.Forms.Label();
            this.listViewResults = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderSize = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderModifiedDate = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderCreatedDate = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderLocation = new System.Windows.Forms.ColumnHeader();
            this.labelSearchLocation = new System.Windows.Forms.Label();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.labelTotalFiles = new System.Windows.Forms.Label();
            this.labelTotalFilesValue = new System.Windows.Forms.Label();
            this.labelTotalDirsValue = new System.Windows.Forms.Label();
            this.labelTotalDirs = new System.Windows.Forms.Label();
            this.labelFoundValue = new System.Windows.Forms.Label();
            this.labelFound = new System.Windows.Forms.Label();
            this.groupBoxResults = new System.Windows.Forms.GroupBox();
            this.labelProcessedValue = new System.Windows.Forms.Label();
            this.labelProcessed = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.labelContainsText = new System.Windows.Forms.Label();
            this.comboBoxSize = new System.Windows.Forms.ComboBox();
            this.textBoxSizeGreater = new System.Windows.Forms.TextBox();
            this.textBoxSizeLess = new System.Windows.Forms.TextBox();
            this.dateTimePickerModifiedBeforeDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerModifiedBeforeTime = new System.Windows.Forms.DateTimePicker();
            this.labelModified = new System.Windows.Forms.Label();
            this.dateTimePickerModifiedAfterTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerModifiedAfterDate = new System.Windows.Forms.DateTimePicker();
            this.groupBoxCritera = new System.Windows.Forms.GroupBox();
            this.checkBoxModifiedByAfter = new System.Windows.Forms.CheckBox();
            this.checkBoxModifiedByBefore = new System.Windows.Forms.CheckBox();
            this.checkBoxSizeLess = new System.Windows.Forms.CheckBox();
            this.buttonContainsTextCaseSensitive = new System.Windows.Forms.Button();
            this.checkBoxSizeGreater = new System.Windows.Forms.CheckBox();
            this.buttonPatternCaseSensitive = new System.Windows.Forms.Button();
            this.comboBoxLocation = new System.Windows.Forms.ComboBox();
            this.comboBoxPattern = new System.Windows.Forms.ComboBox();
            this.comboBoxContainsText = new System.Windows.Forms.ComboBox();
            this.splitContainerVert = new System.Windows.Forms.SplitContainer();
            this.groupBoxResults.SuspendLayout();
            this.groupBoxCritera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVert)).BeginInit();
            this.splitContainerVert.Panel1.SuspendLayout();
            this.splitContainerVert.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelSearchPattern
            // 
            this.labelSearchPattern.AutoSize = true;
            this.labelSearchPattern.Location = new System.Drawing.Point(38, 20);
            this.labelSearchPattern.Name = "labelSearchPattern";
            this.labelSearchPattern.Size = new System.Drawing.Size(60, 15);
            this.labelSearchPattern.TabIndex = 0;
            this.labelSearchPattern.Text = "File Name";
            // 
            // listViewResults
            // 
            this.listViewResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderSize,
            this.columnHeaderModifiedDate,
            this.columnHeaderCreatedDate,
            this.columnHeaderLocation});
            this.listViewResults.FullRowSelect = true;
            this.listViewResults.GridLines = true;
            this.listViewResults.Location = new System.Drawing.Point(8, 205);
            this.listViewResults.Name = "listViewResults";
            this.listViewResults.Size = new System.Drawing.Size(876, 317);
            this.listViewResults.TabIndex = 2;
            this.listViewResults.UseCompatibleStateImageBehavior = false;
            this.listViewResults.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 250;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Size";
            this.columnHeaderSize.Width = 100;
            // 
            // columnHeaderModifiedDate
            // 
            this.columnHeaderModifiedDate.Text = "Modified Date";
            this.columnHeaderModifiedDate.Width = 100;
            // 
            // columnHeaderCreatedDate
            // 
            this.columnHeaderCreatedDate.Text = "Created Date";
            this.columnHeaderCreatedDate.Width = 100;
            // 
            // columnHeaderLocation
            // 
            this.columnHeaderLocation.Text = "Location";
            this.columnHeaderLocation.Width = 250;
            // 
            // labelSearchLocation
            // 
            this.labelSearchLocation.AutoSize = true;
            this.labelSearchLocation.Location = new System.Drawing.Point(7, 78);
            this.labelSearchLocation.Name = "labelSearchLocation";
            this.labelSearchLocation.Size = new System.Drawing.Size(91, 15);
            this.labelSearchLocation.TabIndex = 5;
            this.labelSearchLocation.Text = "Search Location";
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(644, 74);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(27, 23);
            this.buttonBrowse.TabIndex = 6;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // labelTotalFiles
            // 
            this.labelTotalFiles.AutoSize = true;
            this.labelTotalFiles.Location = new System.Drawing.Point(15, 22);
            this.labelTotalFiles.Name = "labelTotalFiles";
            this.labelTotalFiles.Size = new System.Drawing.Size(58, 15);
            this.labelTotalFiles.TabIndex = 8;
            this.labelTotalFiles.Text = "Total Files";
            // 
            // labelTotalFilesValue
            // 
            this.labelTotalFilesValue.AutoSize = true;
            this.labelTotalFilesValue.Location = new System.Drawing.Point(108, 22);
            this.labelTotalFilesValue.Name = "labelTotalFilesValue";
            this.labelTotalFilesValue.Size = new System.Drawing.Size(13, 15);
            this.labelTotalFilesValue.TabIndex = 9;
            this.labelTotalFilesValue.Text = "0";
            // 
            // labelTotalDirsValue
            // 
            this.labelTotalDirsValue.AutoSize = true;
            this.labelTotalDirsValue.Location = new System.Drawing.Point(108, 39);
            this.labelTotalDirsValue.Name = "labelTotalDirsValue";
            this.labelTotalDirsValue.Size = new System.Drawing.Size(13, 15);
            this.labelTotalDirsValue.TabIndex = 11;
            this.labelTotalDirsValue.Text = "0";
            // 
            // labelTotalDirs
            // 
            this.labelTotalDirs.AutoSize = true;
            this.labelTotalDirs.Location = new System.Drawing.Point(15, 39);
            this.labelTotalDirs.Name = "labelTotalDirs";
            this.labelTotalDirs.Size = new System.Drawing.Size(91, 15);
            this.labelTotalDirs.TabIndex = 10;
            this.labelTotalDirs.Text = "Total Directories";
            // 
            // labelFoundValue
            // 
            this.labelFoundValue.AutoSize = true;
            this.labelFoundValue.Location = new System.Drawing.Point(108, 56);
            this.labelFoundValue.Name = "labelFoundValue";
            this.labelFoundValue.Size = new System.Drawing.Size(13, 15);
            this.labelFoundValue.TabIndex = 13;
            this.labelFoundValue.Text = "0";
            // 
            // labelFound
            // 
            this.labelFound.AutoSize = true;
            this.labelFound.Location = new System.Drawing.Point(15, 56);
            this.labelFound.Name = "labelFound";
            this.labelFound.Size = new System.Drawing.Size(41, 15);
            this.labelFound.TabIndex = 12;
            this.labelFound.Text = "Found";
            // 
            // groupBoxResults
            // 
            this.groupBoxResults.Controls.Add(this.labelProcessedValue);
            this.groupBoxResults.Controls.Add(this.labelProcessed);
            this.groupBoxResults.Controls.Add(this.labelTotalFiles);
            this.groupBoxResults.Controls.Add(this.labelFoundValue);
            this.groupBoxResults.Controls.Add(this.labelTotalFilesValue);
            this.groupBoxResults.Controls.Add(this.labelFound);
            this.groupBoxResults.Controls.Add(this.labelTotalDirs);
            this.groupBoxResults.Controls.Add(this.labelTotalDirsValue);
            this.groupBoxResults.Location = new System.Drawing.Point(703, 8);
            this.groupBoxResults.Name = "groupBoxResults";
            this.groupBoxResults.Size = new System.Drawing.Size(181, 100);
            this.groupBoxResults.TabIndex = 14;
            this.groupBoxResults.TabStop = false;
            this.groupBoxResults.Text = "Results";
            // 
            // labelProcessedValue
            // 
            this.labelProcessedValue.AutoSize = true;
            this.labelProcessedValue.Location = new System.Drawing.Point(108, 73);
            this.labelProcessedValue.Name = "labelProcessedValue";
            this.labelProcessedValue.Size = new System.Drawing.Size(13, 15);
            this.labelProcessedValue.TabIndex = 15;
            this.labelProcessedValue.Text = "0";
            // 
            // labelProcessed
            // 
            this.labelProcessed.AutoSize = true;
            this.labelProcessed.Location = new System.Drawing.Point(15, 73);
            this.labelProcessed.Name = "labelProcessed";
            this.labelProcessed.Size = new System.Drawing.Size(60, 15);
            this.labelProcessed.TabIndex = 14;
            this.labelProcessed.Text = "Processed";
            // 
            // buttonSearch
            // 
            this.buttonSearch.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonSearch.Location = new System.Drawing.Point(703, 114);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(91, 34);
            this.buttonSearch.TabIndex = 15;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = false;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // labelContainsText
            // 
            this.labelContainsText.AutoSize = true;
            this.labelContainsText.Location = new System.Drawing.Point(20, 50);
            this.labelContainsText.Name = "labelContainsText";
            this.labelContainsText.Size = new System.Drawing.Size(78, 15);
            this.labelContainsText.TabIndex = 16;
            this.labelContainsText.Text = "Contains Text";
            // 
            // comboBoxSize
            // 
            this.comboBoxSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSize.FormattingEnabled = true;
            this.comboBoxSize.Items.AddRange(new object[] {
            "Size (Byte)",
            "Size (KB)",
            "Size (MB)",
            "Size (GB)"});
            this.comboBoxSize.Location = new System.Drawing.Point(7, 117);
            this.comboBoxSize.Name = "comboBoxSize";
            this.comboBoxSize.Size = new System.Drawing.Size(80, 23);
            this.comboBoxSize.TabIndex = 18;
            // 
            // textBoxSizeGreater
            // 
            this.textBoxSizeGreater.Location = new System.Drawing.Point(118, 117);
            this.textBoxSizeGreater.Name = "textBoxSizeGreater";
            this.textBoxSizeGreater.Size = new System.Drawing.Size(63, 23);
            this.textBoxSizeGreater.TabIndex = 21;
            // 
            // textBoxSizeLess
            // 
            this.textBoxSizeLess.Location = new System.Drawing.Point(212, 117);
            this.textBoxSizeLess.Name = "textBoxSizeLess";
            this.textBoxSizeLess.Size = new System.Drawing.Size(63, 23);
            this.textBoxSizeLess.TabIndex = 22;
            // 
            // dateTimePickerModifiedBeforeDate
            // 
            this.dateTimePickerModifiedBeforeDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerModifiedBeforeDate.Location = new System.Drawing.Point(391, 117);
            this.dateTimePickerModifiedBeforeDate.Name = "dateTimePickerModifiedBeforeDate";
            this.dateTimePickerModifiedBeforeDate.Size = new System.Drawing.Size(90, 23);
            this.dateTimePickerModifiedBeforeDate.TabIndex = 23;
            // 
            // dateTimePickerModifiedBeforeTime
            // 
            this.dateTimePickerModifiedBeforeTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerModifiedBeforeTime.Location = new System.Drawing.Point(390, 146);
            this.dateTimePickerModifiedBeforeTime.Name = "dateTimePickerModifiedBeforeTime";
            this.dateTimePickerModifiedBeforeTime.ShowUpDown = true;
            this.dateTimePickerModifiedBeforeTime.Size = new System.Drawing.Size(91, 23);
            this.dateTimePickerModifiedBeforeTime.TabIndex = 25;
            // 
            // labelModified
            // 
            this.labelModified.AutoSize = true;
            this.labelModified.Location = new System.Drawing.Point(281, 121);
            this.labelModified.Name = "labelModified";
            this.labelModified.Size = new System.Drawing.Size(55, 15);
            this.labelModified.TabIndex = 26;
            this.labelModified.Text = "Modified";
            // 
            // dateTimePickerModifiedAfterTime
            // 
            this.dateTimePickerModifiedAfterTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerModifiedAfterTime.Location = new System.Drawing.Point(535, 146);
            this.dateTimePickerModifiedAfterTime.Name = "dateTimePickerModifiedAfterTime";
            this.dateTimePickerModifiedAfterTime.ShowUpDown = true;
            this.dateTimePickerModifiedAfterTime.Size = new System.Drawing.Size(91, 23);
            this.dateTimePickerModifiedAfterTime.TabIndex = 28;
            // 
            // dateTimePickerModifiedAfterDate
            // 
            this.dateTimePickerModifiedAfterDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerModifiedAfterDate.Location = new System.Drawing.Point(535, 117);
            this.dateTimePickerModifiedAfterDate.Name = "dateTimePickerModifiedAfterDate";
            this.dateTimePickerModifiedAfterDate.Size = new System.Drawing.Size(90, 23);
            this.dateTimePickerModifiedAfterDate.TabIndex = 27;
            // 
            // groupBoxCritera
            // 
            this.groupBoxCritera.Controls.Add(this.checkBoxModifiedByAfter);
            this.groupBoxCritera.Controls.Add(this.checkBoxModifiedByBefore);
            this.groupBoxCritera.Controls.Add(this.checkBoxSizeLess);
            this.groupBoxCritera.Controls.Add(this.buttonContainsTextCaseSensitive);
            this.groupBoxCritera.Controls.Add(this.checkBoxSizeGreater);
            this.groupBoxCritera.Controls.Add(this.buttonPatternCaseSensitive);
            this.groupBoxCritera.Controls.Add(this.comboBoxLocation);
            this.groupBoxCritera.Controls.Add(this.comboBoxPattern);
            this.groupBoxCritera.Controls.Add(this.comboBoxContainsText);
            this.groupBoxCritera.Controls.Add(this.labelSearchPattern);
            this.groupBoxCritera.Controls.Add(this.dateTimePickerModifiedAfterTime);
            this.groupBoxCritera.Controls.Add(this.labelSearchLocation);
            this.groupBoxCritera.Controls.Add(this.dateTimePickerModifiedAfterDate);
            this.groupBoxCritera.Controls.Add(this.buttonBrowse);
            this.groupBoxCritera.Controls.Add(this.labelModified);
            this.groupBoxCritera.Controls.Add(this.labelContainsText);
            this.groupBoxCritera.Controls.Add(this.dateTimePickerModifiedBeforeTime);
            this.groupBoxCritera.Controls.Add(this.dateTimePickerModifiedBeforeDate);
            this.groupBoxCritera.Controls.Add(this.comboBoxSize);
            this.groupBoxCritera.Controls.Add(this.textBoxSizeLess);
            this.groupBoxCritera.Controls.Add(this.textBoxSizeGreater);
            this.groupBoxCritera.Location = new System.Drawing.Point(8, 8);
            this.groupBoxCritera.Name = "groupBoxCritera";
            this.groupBoxCritera.Size = new System.Drawing.Size(675, 178);
            this.groupBoxCritera.TabIndex = 31;
            this.groupBoxCritera.TabStop = false;
            this.groupBoxCritera.Text = "Critera";
            // 
            // checkBoxModifiedByAfter
            // 
            this.checkBoxModifiedByAfter.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxModifiedByAfter.AutoSize = true;
            this.checkBoxModifiedByAfter.Location = new System.Drawing.Point(490, 116);
            this.checkBoxModifiedByAfter.Name = "checkBoxModifiedByAfter";
            this.checkBoxModifiedByAfter.Size = new System.Drawing.Size(43, 25);
            this.checkBoxModifiedByAfter.TabIndex = 0;
            this.checkBoxModifiedByAfter.Text = "After";
            this.checkBoxModifiedByAfter.UseVisualStyleBackColor = true;
            this.checkBoxModifiedByAfter.CheckedChanged += new System.EventHandler(this.checkBoxModifiedByAfter_CheckedChanged);
            // 
            // checkBoxModifiedByBefore
            // 
            this.checkBoxModifiedByBefore.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxModifiedByBefore.AutoSize = true;
            this.checkBoxModifiedByBefore.Location = new System.Drawing.Point(338, 116);
            this.checkBoxModifiedByBefore.Name = "checkBoxModifiedByBefore";
            this.checkBoxModifiedByBefore.Size = new System.Drawing.Size(51, 25);
            this.checkBoxModifiedByBefore.TabIndex = 0;
            this.checkBoxModifiedByBefore.Text = "Before";
            this.checkBoxModifiedByBefore.UseVisualStyleBackColor = true;
            this.checkBoxModifiedByBefore.CheckedChanged += new System.EventHandler(this.checkBoxModifiedByBefore_CheckedChanged);
            // 
            // checkBoxSizeLess
            // 
            this.checkBoxSizeLess.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxSizeLess.AutoSize = true;
            this.checkBoxSizeLess.Location = new System.Drawing.Point(186, 116);
            this.checkBoxSizeLess.Name = "checkBoxSizeLess";
            this.checkBoxSizeLess.Size = new System.Drawing.Size(25, 25);
            this.checkBoxSizeLess.TabIndex = 1;
            this.checkBoxSizeLess.Text = "<";
            this.checkBoxSizeLess.UseVisualStyleBackColor = true;
            this.checkBoxSizeLess.CheckedChanged += new System.EventHandler(this.checkBoxSizeLess_CheckedChanged);
            // 
            // buttonContainsTextCaseSensitive
            // 
            this.buttonContainsTextCaseSensitive.FlatAppearance.BorderSize = 0;
            this.buttonContainsTextCaseSensitive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonContainsTextCaseSensitive.Image = global::FinderSeeker.Properties.Resources.CaseSensitiveNotSelected;
            this.buttonContainsTextCaseSensitive.Location = new System.Drawing.Point(647, 47);
            this.buttonContainsTextCaseSensitive.Name = "buttonContainsTextCaseSensitive";
            this.buttonContainsTextCaseSensitive.Size = new System.Drawing.Size(20, 20);
            this.buttonContainsTextCaseSensitive.TabIndex = 35;
            this.buttonContainsTextCaseSensitive.UseVisualStyleBackColor = false;
            this.buttonContainsTextCaseSensitive.Click += new System.EventHandler(this.buttonContainsTextCaseSensitive_Click);
            // 
            // checkBoxSizeGreater
            // 
            this.checkBoxSizeGreater.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxSizeGreater.AutoSize = true;
            this.checkBoxSizeGreater.Location = new System.Drawing.Point(92, 116);
            this.checkBoxSizeGreater.Name = "checkBoxSizeGreater";
            this.checkBoxSizeGreater.Size = new System.Drawing.Size(25, 25);
            this.checkBoxSizeGreater.TabIndex = 0;
            this.checkBoxSizeGreater.Text = ">";
            this.checkBoxSizeGreater.UseVisualStyleBackColor = true;
            this.checkBoxSizeGreater.CheckedChanged += new System.EventHandler(this.checkBoxSizeGreater_CheckedChanged);
            // 
            // buttonPatternCaseSensitive
            // 
            this.buttonPatternCaseSensitive.FlatAppearance.BorderSize = 0;
            this.buttonPatternCaseSensitive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPatternCaseSensitive.Image = global::FinderSeeker.Properties.Resources.CaseSensitiveNotSelected;
            this.buttonPatternCaseSensitive.Location = new System.Drawing.Point(647, 17);
            this.buttonPatternCaseSensitive.Name = "buttonPatternCaseSensitive";
            this.buttonPatternCaseSensitive.Size = new System.Drawing.Size(20, 20);
            this.buttonPatternCaseSensitive.TabIndex = 34;
            this.buttonPatternCaseSensitive.UseVisualStyleBackColor = false;
            this.buttonPatternCaseSensitive.Click += new System.EventHandler(this.buttonPatternCaseSensitive_Click);
            // 
            // comboBoxLocation
            // 
            this.comboBoxLocation.FormattingEnabled = true;
            this.comboBoxLocation.Location = new System.Drawing.Point(104, 75);
            this.comboBoxLocation.Name = "comboBoxLocation";
            this.comboBoxLocation.Size = new System.Drawing.Size(535, 23);
            this.comboBoxLocation.TabIndex = 33;
            // 
            // comboBoxPattern
            // 
            this.comboBoxPattern.FormattingEnabled = true;
            this.comboBoxPattern.Location = new System.Drawing.Point(104, 17);
            this.comboBoxPattern.Name = "comboBoxPattern";
            this.comboBoxPattern.Size = new System.Drawing.Size(535, 23);
            this.comboBoxPattern.TabIndex = 32;
            // 
            // comboBoxContainsText
            // 
            this.comboBoxContainsText.FormattingEnabled = true;
            this.comboBoxContainsText.Location = new System.Drawing.Point(104, 47);
            this.comboBoxContainsText.Name = "comboBoxContainsText";
            this.comboBoxContainsText.Size = new System.Drawing.Size(535, 23);
            this.comboBoxContainsText.TabIndex = 32;
            // 
            // splitContainerVert
            // 
            this.splitContainerVert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerVert.Location = new System.Drawing.Point(0, 0);
            this.splitContainerVert.Name = "splitContainerVert";
            // 
            // splitContainerVert.Panel1
            // 
            this.splitContainerVert.Panel1.Controls.Add(this.groupBoxCritera);
            this.splitContainerVert.Panel1.Controls.Add(this.listViewResults);
            this.splitContainerVert.Panel1.Controls.Add(this.buttonSearch);
            this.splitContainerVert.Panel1.Controls.Add(this.groupBoxResults);
            this.splitContainerVert.Panel2Collapsed = true;
            this.splitContainerVert.Size = new System.Drawing.Size(894, 538);
            this.splitContainerVert.SplitterDistance = 869;
            this.splitContainerVert.TabIndex = 32;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 538);
            this.Controls.Add(this.splitContainerVert);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(910, 577);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Finder Seeker";
            this.groupBoxResults.ResumeLayout(false);
            this.groupBoxResults.PerformLayout();
            this.groupBoxCritera.ResumeLayout(false);
            this.groupBoxCritera.PerformLayout();
            this.splitContainerVert.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerVert)).EndInit();
            this.splitContainerVert.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Label labelSearchPattern;
        private ListView listViewResults;
        private Label labelSearchLocation;
        private Button buttonBrowse;
        private Label labelTotalFiles;
        private Label labelTotalFilesValue;
        private Label labelTotalDirsValue;
        private Label labelTotalDirs;
        private Label labelFoundValue;
        private Label labelFound;
        private GroupBox groupBoxResults;
        private Button buttonSearch;
        private ColumnHeader columnHeaderName;
        private ColumnHeader columnHeaderSize;
        private ColumnHeader columnHeaderModifiedDate;
        private ColumnHeader columnHeaderCreatedDate;
        private ColumnHeader columnHeaderLocation;
        private Label labelContainsText;
        private Label labelProcessedValue;
        private Label labelProcessed;
        private ComboBox comboBoxSize;
        private TextBox textBoxSizeGreater;
        private TextBox textBoxSizeLess;
        private DateTimePicker dateTimePickerModifiedBeforeDate;
        private DateTimePicker dateTimePickerModifiedBeforeTime;
        private Label labelModified;
        private DateTimePicker dateTimePickerModifiedAfterTime;
        private DateTimePicker dateTimePickerModifiedAfterDate;
        private GroupBox groupBoxCritera;
        private ComboBox comboBoxLocation;
        private ComboBox comboBoxPattern;
        private ComboBox comboBoxContainsText;
        private Button buttonContainsTextCaseSensitive;
        private Button buttonPatternCaseSensitive;
        private SplitContainer splitContainerVert;
        private CheckBox checkBoxSizeLess;
        private CheckBox checkBoxSizeGreater;
        private CheckBox checkBoxModifiedByAfter;
        private CheckBox checkBoxModifiedByBefore;
    }
}