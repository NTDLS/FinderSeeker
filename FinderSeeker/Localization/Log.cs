using System.Text;

namespace FinderSeeker
{
    /// <summary>
    /// A small file logger. Logging never throws.
    /// </summary>
    public static class Log
    {
        private static readonly object _lock = new();

        public static string Directory { get; set; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NTDLS", "FinderSeeker", "Logs");

        public static string CurrentFile => Path.Combine(Directory, $"finderseeker-{DateTime.Now:yyyyMMdd}.log");

        public static void Info(string message) => Write("INFO ", message, null);
        public static void Warning(string message, Exception? exception = null) => Write("WARN ", message, exception);
        public static void Error(string message, Exception? exception = null) => Write("ERROR", message, exception);

        private static void Write(string level, string message, Exception? exception)
        {
            try
            {
                var text = new StringBuilder();
                text.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {level} {message}");
                if (exception != null)
                {
                    text.AppendLine();
                    text.Append(exception);
                }
                text.AppendLine();

                lock (_lock)
                {
                    System.IO.Directory.CreateDirectory(Directory);
                    File.AppendAllText(CurrentFile, text.ToString());
                }
            }
            catch
            {
                //Logging must never be the reason something fails.
            }
        }
    }
}
