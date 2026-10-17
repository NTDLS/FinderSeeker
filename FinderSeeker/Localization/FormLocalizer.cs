using System.Runtime.CompilerServices;

namespace FinderSeeker.Localization
{
    /// <summary>
    /// Translates the text the Windows Forms designer put on a form: titles, labels, buttons, check boxes, group boxes, tab pages,
    /// menus, list columns and text box placeholders. Designer code is generated, so it cannot hold Localize(...) calls. Instead the
    /// designer's text is used as the phrase, and is replaced by its translation when the form is created.
    ///
    /// Call it from a form's constructor after InitializeComponent(), and again when the language is changed: the designer's original
    /// text is remembered, so the form is translated from English each time. Text that the code has set itself is left alone, so it
    /// must be updated by the form.
    ///
    /// LocalizerScan reads the same designer text, so the phrases get into the translation database.
    /// </summary>
    public static class FormLocalizer
    {
        private sealed class Remembered
        {
            public string Source = string.Empty;
            public string Applied = string.Empty;
            public bool ChangedByCode;
        }

        private static readonly ConditionalWeakTable<object, Dictionary<string, Remembered>> _remembered = new();

        public static void Apply(Control root) => Apply(root, Localizer.Default);

        public static void Apply(Control root, Localizer localizer) => Translate(root, localizer, new HashSet<object>());

        /// <summary>
        /// The designer's original (English) text of a control, item or column, or null if it was not seen by <see cref="Apply(Control)"/>.
        /// </summary>
        public static string? SourceText(object owner, string property = "Text")
        {
            return _remembered.TryGetValue(owner, out var properties) && properties.TryGetValue(property, out var remembered) ? remembered.Source : null;
        }

        private static string Translate(object owner, string property, string current, Localizer localizer)
        {
            var properties = _remembered.GetOrCreateValue(owner);

            if (!properties.TryGetValue(property, out var remembered))
            {
                remembered = new Remembered { Source = current, Applied = current };
                properties[property] = remembered;
            }
            else if (remembered.ChangedByCode || current != remembered.Applied)
            {
                remembered.ChangedByCode = true;
                return current;
            }

            if (!Phrases.IsTranslatable(remembered.Source))
            {
                return current;
            }

            remembered.Applied = localizer.Localize(remembered.Source);
            return remembered.Applied;
        }

        private static void Translate(Control control, Localizer localizer, HashSet<object> done)
        {
            if (!done.Add(control))
            {
                return;
            }

            //Only the controls whose text is wording. The text of a text box, a combo box or a list is the user's or the program's.
            if (control is Form || control is Label || control is ButtonBase || control is GroupBox || control is TabPage)
            {
                control.Text = Translate(control, "Text", control.Text, localizer);
            }

            if (control is TextBox textBox)
            {
                textBox.PlaceholderText = Translate(textBox, "PlaceholderText", textBox.PlaceholderText, localizer);
            }

            if (control is ListView listView)
            {
                foreach (ColumnHeader column in listView.Columns)
                {
                    column.Text = Translate(column, "Text", column.Text, localizer);
                }
            }

            if (control is ToolStrip toolStrip)
            {
                Translate(toolStrip.Items, localizer, done);
            }

            if (control.ContextMenuStrip != null)
            {
                Translate(control.ContextMenuStrip, localizer, done);
            }

            foreach (Control child in control.Controls)
            {
                Translate(child, localizer, done);
            }
        }

        private static void Translate(ToolStripItemCollection items, Localizer localizer, HashSet<object> done)
        {
            foreach (ToolStripItem item in items)
            {
                if (!done.Add(item))
                {
                    continue;
                }

                if (item is not ToolStripSeparator && item is not ToolStripTextBox && item is not ToolStripComboBox)
                {
                    item.Text = Translate(item, "Text", item.Text, localizer);
                }

                if (item is ToolStripDropDownItem dropDown)
                {
                    Translate(dropDown.DropDownItems, localizer, done);
                }
            }
        }
    }
}
