using NTDLS.Localization;
using FinderSeeker.UI;
using NTDLS.WinFormsHelpers;

namespace FinderSeeker
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Anything unhandled is logged (Help > Open log folder) as well as shown.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, e) =>
            {
                Log.Error("Unhandled exception.", e.Exception);
                MessageBox.Show(Localize("An unexpected error occurred:\r\n\r\n{0}\r\n\r\nDetails are in the log (Help > Open log folder).", e.Exception.Message),
                    "FinderSeeker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            AppDomain.CurrentDomain.UnhandledException += (_, e) => Log.Error("Unhandled exception on a background thread.", e.ExceptionObject as Exception);

            // Problems the localization package finds go in the log.
            LocalizationDiagnostics.OnWarning = Log.Warning;
            LocalizationDiagnostics.OnError = Log.Error;

            // The language is the user's choice in the settings file, or else the Windows language.
            Localizer.Default.SetLanguage(AppSettings.Load().Language);

            // Follow the Windows light/dark setting. Must happen before any form is created.
            ApplicationConfiguration.Initialize();
            Theming.Initialize();

            Application.Run(new FormMain());
        }
    }
}
