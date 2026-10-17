using FinderSeeker.Search;
using System.Text;

namespace FinderSeeker.UI
{
    /// <summary>
    /// Shows what is in the selected result: images as pictures, text as text (with the content search's matches highlighted),
    /// anything else as a hex dump of its beginning, .
    /// </summary>
    public sealed class PreviewPanel : Panel
    {
        private const int MaxTextBytes = 1024 * 1024;
        private const int MaxImageBytes = 64 * 1024 * 1024;
        private const int HexDumpBytes = 8 * 1024;
        private const int MaxHighlights = 1000;

        private static readonly HashSet<string> _imageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".bmp", ".png", ".jpg", ".jpeg", ".jpe", ".gif", ".tif", ".tiff", ".ico", ".emf", ".wmf"
        };

        private readonly Label _header = new();
        private readonly Panel _body = new();
        private readonly RichTextBox _text = new();
        private readonly PictureBox _picture = new();
        private readonly Label _message = new();

        private CancellationTokenSource? _cancellation;
        private Image? _image;
        private SearchResult? _current;
        private int _version;

        /// <summary>
        /// The content search in use, so its matches can be highlighted. Null when there is none.
        /// </summary>
        public ContentMatcher? Highlighter { get; set; }

        public PreviewPanel()
        {
            _header.Dock = DockStyle.Top;
            _header.AutoSize = false;
            _header.Height = 52;
            _header.Padding = new Padding(6, 4, 6, 4);

            _body.Dock = DockStyle.Fill;

            _text.Dock = DockStyle.Fill;
            _text.ReadOnly = true;
            _text.WordWrap = false;
            _text.BorderStyle = BorderStyle.None;
            _text.HideSelection = false;
            _text.DetectUrls = false;
            _text.Font = new Font("Consolas", 9.75f);
            _text.BackColor = SystemColors.Window;
            _text.ForeColor = SystemColors.WindowText;
            _text.Visible = false;

            _picture.Dock = DockStyle.Fill;
            _picture.SizeMode = PictureBoxSizeMode.Zoom;
            _picture.Visible = false;

            _message.Dock = DockStyle.Fill;
            _message.TextAlign = ContentAlignment.MiddleCenter;
            _message.ForeColor = Theming.Muted;
            _message.Padding = new Padding(12);

            _body.Controls.Add(_message);
            _body.Controls.Add(_picture);
            _body.Controls.Add(_text);

            Controls.Add(_body);
            Controls.Add(_header);

            ShowMessage(Localize("Select a result to preview it."));
        }

        /// <summary>
        /// Shows a result, or clears the panel when it is null.
        /// </summary>
        public async void ShowResult(SearchResult? result)
        {
            _cancellation?.Cancel();
            _cancellation = new CancellationTokenSource();
            var token = _cancellation.Token;
            int version = ++_version;
            _current = result;

            if (result == null)
            {
                _header.Text = string.Empty;
                ShowMessage(Localize("Select a result to preview it."));
                return;
            }

            _header.Text = DescribeHeader(result, null);

            try
            {
                var path = result.FullPath;
                var extension = Path.GetExtension(result.Name);

                if (_imageExtensions.Contains(extension))
                {
                    var bytes = await Task.Run(() => ReadBytes(path, MaxImageBytes), token);
                    if (version != _version)
                    {
                        return;
                    }

                    ShowImage(bytes, result);
                    return;
                }

                var head = await Task.Run(() => ReadBytes(path, MaxTextBytes), token);
                if (version != _version)
                {
                    return;
                }

                if (head.Length == 0)
                {
                    ShowMessage(Localize("This file is empty."));
                    return;
                }

                if (LooksLikeText(head, out var encoding))
                {
                    bool truncated = result.Length > head.Length;
                    var text = encoding.GetString(head, Preamble(head, encoding), head.Length - Preamble(head, encoding));
                    var highlights = Highlighter == null ? null : await Task.Run(() => Highlighter.FindAll(text, MaxHighlights), token);
                    if (version != _version)
                    {
                        return;
                    }

                    ShowText(text, highlights);
                    _header.Text = DescribeHeader(result, truncated ? Localize("Showing the first {0}", FormatSize(head.Length)) : null);
                }
                else
                {
                    var dump = await Task.Run(() => HexDump(head.AsSpan(0, Math.Min(head.Length, HexDumpBytes))), token);
                    if (version != _version)
                    {
                        return;
                    }

                    ShowText(dump, null);
                    _header.Text = DescribeHeader(result, Localize("Binary file: showing the first {0} as hex", FormatSize(Math.Min(head.Length, HexDumpBytes))));
                }
            }
            catch (OperationCanceledException)
            {
                //Another result was selected.
            }
            catch (Exception ex) when (version == _version)
            {
                ShowMessage(Localize("This file cannot be previewed: {0}", ex.Message));
            }
        }

        private static string FormatSize(long bytes) => Format.FileSize(bytes);

        private string DescribeHeader(SearchResult result, string? note)
        {
            var size = Format.FileSize(result.Length) + "   ";
            var text = $"{result.Name}\r\n{size}{result.Modified:yyyy-MM-dd HH:mm}";
            return note == null ? text : text + "   " + note;
        }

        private void ShowMessage(string message)
        {
            ReleaseImage();
            _text.Visible = false;
            _picture.Visible = false;
            _message.Text = message;
            _message.Visible = true;
        }

        private void ShowText(string text, List<(int Index, int Length)>? highlights)
        {
            ReleaseImage();
            _picture.Visible = false;
            _message.Visible = false;
            _text.Visible = true;

            _text.SuspendLayout();
            _text.Clear();
            _text.Text = text;
            _text.SelectAll();
            _text.SelectionBackColor = SystemColors.Window;
            _text.SelectionColor = SystemColors.WindowText;

            if (highlights != null)
            {
                foreach (var (index, length) in highlights)
                {
                    _text.Select(index, length);
                    _text.SelectionBackColor = Theming.Highlight;
                    _text.SelectionColor = SystemColors.WindowText;
                }
            }

            _text.Select(highlights is { Count: > 0 } ? highlights[0].Index : 0, 0);
            _text.ScrollToCaret();
            _text.ResumeLayout();
        }

        private void ShowImage(byte[] bytes, SearchResult result)
        {
            ReleaseImage();

            try
            {
                var stream = new MemoryStream(bytes);
                _image = Image.FromStream(stream);
            }
            catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException)
            {
                ShowMessage(Localize("This image cannot be shown."));
                return;
            }

            _text.Visible = false;
            _message.Visible = false;
            _picture.Image = _image;
            _picture.SizeMode = _image.Width <= _picture.Width && _image.Height <= _picture.Height
                ? PictureBoxSizeMode.CenterImage : PictureBoxSizeMode.Zoom;
            _picture.Visible = true;

            _header.Text = DescribeHeader(result, $"{_image.Width} × {_image.Height}");
        }

        private void ReleaseImage()
        {
            _picture.Image = null;
            _image?.Dispose();
            _image = null;
        }

        private static byte[] ReadBytes(string path, int limit)
        {
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            var length = (int)Math.Min(stream.Length, limit);
            var bytes = new byte[length];
            int total = 0;
            while (total < length)
            {
                int read = stream.Read(bytes, total, length - total);
                if (read <= 0)
                {
                    break;
                }
                total += read;
            }

            return total == length ? bytes : bytes[..total];
        }

        private static bool LooksLikeText(byte[] bytes, out Encoding encoding)
        {
            encoding = new UTF8Encoding(false);

            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                return true;
            }
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
            {
                encoding = Encoding.Unicode;
                return true;
            }
            if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
            {
                encoding = Encoding.BigEndianUnicode;
                return true;
            }

            return !bytes.AsSpan(0, Math.Min(bytes.Length, 8192)).Contains((byte)0);
        }

        private static int Preamble(byte[] bytes, Encoding encoding)
        {
            var preamble = encoding.GetPreamble();
            if (preamble.Length > 0 && bytes.AsSpan().StartsWith(preamble))
            {
                return preamble.Length;
            }

            //UTF-8 with a byte order mark: the encoding above does not write one but must skip one.
            return bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF ? 3 : 0;
        }

        private static string HexDump(ReadOnlySpan<byte> bytes)
        {
            var text = new StringBuilder(bytes.Length * 4);
            for (int offset = 0; offset < bytes.Length; offset += 16)
            {
                var row = bytes.Slice(offset, Math.Min(16, bytes.Length - offset));
                text.Append(offset.ToString("X8")).Append("  ");

                for (int i = 0; i < 16; i++)
                {
                    text.Append(i < row.Length ? row[i].ToString("X2") : "  ").Append(i == 7 ? "  " : " ");
                }

                text.Append(" ");
                foreach (var b in row)
                {
                    text.Append(b >= 32 && b < 127 ? (char)b : '.');
                }
                text.AppendLine();
            }

            return text.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cancellation?.Cancel();
                ReleaseImage();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Text for sizes.
    /// </summary>
    public static class Format
    {
        public static string FileSize(long bytes)
        {
            string[] units = ["B", "KB", "MB", "GB", "TB", "PB"];
            double value = bytes;
            int unit = 0;
            while (value >= 1024 && unit < units.Length - 1)
            {
                value /= 1024;
                unit++;
            }

            return unit == 0 ? $"{bytes:N0} B" : $"{value:N1} {units[unit]}";
        }
    }
}
