using FinderSeeker.Search;
using NTDLS.WinFormsHelpers;

namespace FinderSeeker.UI
{
    /// <summary>
    /// The Properties tab of the side panel: what is known about the selected file, in groups.
    /// </summary>
    public partial class FormMain
    {
        private CancellationTokenSource? _propertiesCancellation;
        private int _propertiesVersion;
        private readonly List<PropertyItem> _properties = new();

        /// <summary>
        /// Prepares the list: two columns, grouped rows. Called once, from the constructor.
        /// </summary>
        private void SetupProperties()
        {
            listViewProperties.View = View.Details;
            listViewProperties.ShowGroups = false; // the native group headers are unreadable in dark mode, so groups are heading rows
            listViewProperties.FullRowSelect = true;
            listViewProperties.MultiSelect = true;
            listViewProperties.HideSelection = false;
            listViewProperties.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listViewProperties.BorderStyle = BorderStyle.None;
            listViewProperties.ShowItemToolTips = true;

            listViewProperties.Columns.Clear();
            listViewProperties.Columns.Add(new ColumnHeader { Width = 170 });
            listViewProperties.Columns.Add(new ColumnHeader { Width = 240 });

            listViewProperties.SizeChanged += (_, _) => FitPropertyColumns();
            listViewProperties.KeyDown += listViewProperties_KeyDown;
            tabControlPreview.SelectedIndexChanged += (_, _) => RefreshDetails();
        }

        private void FitPropertyColumns()
        {
            if (listViewProperties.Columns.Count == 2)
            {
                listViewProperties.Columns[1].Width = Math.Max(150, listViewProperties.ClientSize.Width - listViewProperties.Columns[0].Width - 2);
            }
        }

        private void LocalizeProperties()
        {
            if (listViewProperties.Columns.Count == 2)
            {
                listViewProperties.Columns[0].Text = Localize("Property");
                listViewProperties.Columns[1].Text = Localize("Value");
            }
        }

        /// <summary>
        /// Shows the selected result in whichever tab of the side panel is in front.
        /// </summary>
        private void RefreshDetails()
        {
            var result = SelectedResult();

            if (tabControlPreview.SelectedTab == tabPageProperties)
            {
                ShowProperties(result);
            }
            else
            {
                previewPanel.ShowResult(result);
            }
        }

        private async void ShowProperties(SearchResult? result)
        {
            _propertiesCancellation?.Cancel();
            _propertiesCancellation = new CancellationTokenSource();
            var token = _propertiesCancellation.Token;
            int version = ++_propertiesVersion;

            _properties.Clear();
            Rebuild();

            if (result == null)
            {
                return;
            }

            try
            {
                var basic = await Task.Run(() => FileProperties.Basic(result, token), token);
                if (version != _propertiesVersion)
                {
                    return;
                }
                _properties.AddRange(basic);

                // The checksums need the whole file read, so they come in afterwards.
                var pending = new PropertyItem("Checksums", Localize("Checksums"), Localize("Calculating..."));
                _properties.Add(pending);
                Rebuild();
                var deep = await Task.Run(() => FileProperties.Deep(result, token), token);
                if (version != _propertiesVersion)
                {
                    return;
                }

                _properties.Remove(pending);
                _properties.AddRange(deep);
                Rebuild();
            }
            catch (OperationCanceledException)
            {
                // Another file was selected.
            }
        }

        /// <summary>
        /// Fills the list from the collected properties: a heading row for each group, in the fixed order, then its rows.
        /// </summary>
        private void Rebuild()
        {
            listViewProperties.BeginUpdate();
            listViewProperties.Items.Clear();

            var headingFont = new Font(listViewProperties.Font, FontStyle.Bold);
            var headingBack = Theming.IsDarkMode ? Color.FromArgb(0x3A, 0x3A, 0x3A) : SystemColors.ControlLight;

            foreach (var group in _properties.GroupBy(p => p.Group).OrderBy(g => Array.IndexOf(FileProperties.GroupOrder, g.Key)))
            {
                var heading = new ListViewItem(FileProperties.GroupTitle(group.Key))
                {
                    Font = headingFont,
                    BackColor = headingBack,
                    ForeColor = SystemColors.WindowText,
                    Tag = group.Key
                };
                heading.SubItems.Add(string.Empty);
                listViewProperties.Items.Add(heading);

                foreach (var property in group)
                {
                    var item = new ListViewItem(property.Name);
                    item.SubItems.Add(property.Value);
                    listViewProperties.Items.Add(item);
                }
            }

            listViewProperties.EndUpdate();
        }

        private void listViewProperties_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C && listViewProperties.SelectedItems.Count > 0)
            {
                var lines = listViewProperties.SelectedItems.Cast<ListViewItem>().Where(i => i.Tag == null).Select(i => $"{i.Text}\t{i.SubItems[1].Text}");
                try
                {
                    Clipboard.SetText(string.Join(Environment.NewLine, lines));
                }
                catch (System.Runtime.InteropServices.ExternalException ex)
                {
                    Log.Warning("The clipboard was busy.", ex);
                }
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                foreach (ListViewItem item in listViewProperties.Items)
                {
                    item.Selected = true;
                }
                e.Handled = true;
            }
        }
    }
}
