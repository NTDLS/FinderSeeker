namespace FinderSeeker
{
    /// <summary>
    /// Support for WinForms dark mode, which follows the Windows "app mode" setting (same approach as SQL Script Generator).
    /// </summary>
    public static class Theming
    {
        public static bool IsDarkMode => Application.IsDarkModeEnabled;

        /// <summary>
        /// Follows the Windows light/dark setting. Must be called before any form is created.
        /// </summary>
        public static void Initialize() => Application.SetColorMode(SystemColorMode.System);

        public static readonly Color LinkColor = Color.FromArgb(0x8A, 0xB4, 0xF8);
        public static readonly Color ActiveLinkColor = Color.FromArgb(0xB8, 0xD2, 0xFB);
        public static readonly Color VisitedLinkColor = Color.FromArgb(0xC5, 0x8A, 0xF9);

        /// <summary>
        /// Colors for the few things that are painted by hand.
        /// </summary>
        public static Color Highlight => IsDarkMode ? Color.FromArgb(0x6B, 0x5B, 0x00) : Color.FromArgb(0xFF, 0xEB, 0x7A);
        public static Color Muted => IsDarkMode ? Color.FromArgb(0x9A, 0x9A, 0x9A) : Color.FromArgb(0x60, 0x60, 0x60);

        /// <summary>
        /// Works around controls that WinForms dark mode does not fully restyle. Applies to the control, its children, and any added later.
        /// </summary>
        public static void ApplyDarkModeFixups(Control root)
        {
            if (!IsDarkMode)
            {
                return;
            }

            if (root is TabPage tabPage)
            {
                tabPage.UseVisualStyleBackColor = false;
                tabPage.BackColor = SystemColors.Control;
            }
            else if (root is LinkLabel linkLabel)
            {
                linkLabel.LinkColor = LinkColor;
                linkLabel.ActiveLinkColor = ActiveLinkColor;
                linkLabel.VisitedLinkColor = VisitedLinkColor;
            }

            foreach (Control child in root.Controls)
            {
                ApplyDarkModeFixups(child);
            }

            root.ControlAdded -= Root_ControlAdded;
            root.ControlAdded += Root_ControlAdded;
        }

        private static void Root_ControlAdded(object? sender, ControlEventArgs e)
        {
            if (e.Control != null)
            {
                ApplyDarkModeFixups(e.Control);
            }
        }
    }
}
