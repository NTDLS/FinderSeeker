using FinderSeeker.Search;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.Unicode;

namespace FinderSeeker.UI
{
    /// <summary>
    /// One line of the properties list: the group it is shown under (an English key, see <see cref="FileProperties.GroupTitle"/>), its name and its value.
    /// </summary>
    public sealed record PropertyItem(string Group, string Name, string Value);

    /// <summary>
    /// Collects what can be known about a file. <see cref="Basic"/> is quick (it only asks the file system and reads the start of the
    /// file); <see cref="Deep"/> reads the whole file (checksums, line count) and so is done afterwards.
    /// </summary>
    public static class FileProperties
    {
        /// <summary>The groups, in the order they are shown.</summary>
        public static readonly string[] GroupOrder =
            ["Search match", "General", "Dates", "Attributes", "Content", "Version", "Image", "Security", "Drive", "Checksums"];

        public static string GroupTitle(string group) => group switch
        {
            "Search match" => Localize("Search match"),
            "General" => Localize("General"),
            "Dates" => Localize("Dates"),
            "Attributes" => Localize("Attributes"),
            "Content" => Localize("Content"),
            "Version" => Localize("Version information"),
            "Image" => Localize("Image"),
            "Security" => Localize("Security"),
            "Drive" => Localize("Drive"),
            "Checksums" => Localize("Checksums"),
            _ => group
        };

        private const long MaxHashSize = 512L * 1024 * 1024;

        #region Quick.

        public static List<PropertyItem> Basic(SearchResult result, CancellationToken token)
        {
            var items = new List<PropertyItem>();
            var path = result.FullPath;

            void Add(string group, string name, string? value)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    items.Add(new PropertyItem(group, name, value));
                }
            }

            if (result.Line > 0)
            {
                Add("Search match", Localize("Line"), result.Line.ToString("N0"));
                Add("Search match", Localize("Text"), result.Snippet);
            }

            FileInfo info;
            try
            {
                info = new FileInfo(path);
                if (!info.Exists)
                {
                    Add("General", Localize("Status"), Localize("The file no longer exists."));
                    Add("General", Localize("Full path"), path);
                    return items;
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException or NotSupportedException)
            {
                Add("General", Localize("Status"), ex.Message);
                return items;
            }

            // General.
            Add("General", Localize("Name"), info.Name);
            Add("General", Localize("Name without extension"), Path.GetFileNameWithoutExtension(info.Name));
            Add("General", Localize("Extension"), info.Extension);
            Add("General", Localize("Type"), ShellIcons.TypeName(info.Extension));
            Add("General", Localize("Folder"), info.DirectoryName);
            Add("General", Localize("Full path"), info.FullName);
            Add("General", Localize("Path length"), info.FullName.Length.ToString("N0"));
            Add("General", Localize("Size"), $"{Format.FileSize(info.Length)}  ({info.Length:N0} {Localize("bytes")})");
            var onDisk = SizeOnDisk(path);
            if (onDisk != null && onDisk != info.Length)
            {
                Add("General", Localize("Size on disk"), $"{Format.FileSize(onDisk.Value)}  ({onDisk.Value:N0} {Localize("bytes")})");
            }
            if (info.LinkTarget != null)
            {
                Add("General", Localize("Link target"), info.LinkTarget);
            }

            // Dates.
            Add("Dates", Localize("Created"), FormatDate(info.CreationTime));
            Add("Dates", Localize("Modified"), FormatDate(info.LastWriteTime));
            Add("Dates", Localize("Accessed"), FormatDate(info.LastAccessTime));
            Add("Dates", Localize("Modified (UTC)"), info.LastWriteTimeUtc.ToString("yyyy-MM-dd HH:mm:ss"));
            Add("Dates", Localize("Age"), FormatAge(DateTime.Now - info.LastWriteTime));

            // Attributes.
            var attributes = info.Attributes;
            void Flag(string name, FileAttributes flag) => Add("Attributes", name, YesNo((attributes & flag) != 0));
            Flag(Localize("Read-only"), FileAttributes.ReadOnly);
            Flag(Localize("Hidden"), FileAttributes.Hidden);
            Flag(Localize("System"), FileAttributes.System);
            Flag(Localize("Archive"), FileAttributes.Archive);
            Flag(Localize("Compressed"), FileAttributes.Compressed);
            Flag(Localize("Encrypted"), FileAttributes.Encrypted);
            Flag(Localize("Temporary"), FileAttributes.Temporary);
            Flag(Localize("Offline"), FileAttributes.Offline);
            Flag(Localize("Sparse file"), FileAttributes.SparseFile);
            Flag(Localize("Reparse point"), FileAttributes.ReparsePoint);
            Flag(Localize("Excluded from content indexing"), FileAttributes.NotContentIndexed);
            Add("Attributes", Localize("Raw value"), $"0x{(int)attributes:X8}");

            token.ThrowIfCancellationRequested();

            // Content: what the start of the file says.
            byte[] head = ReadHead(path, 64 * 1024);
            Add("Content", Localize("Detected format"), DetectFormat(head, info.Extension));
            if (head.Length > 0)
            {
                var textKind = DescribeText(head, out var isText);
                Add("Content", Localize("Looks like"), isText ? Localize("Text") : Localize("Binary data"));
                if (isText)
                {
                    Add("Content", Localize("Encoding"), textKind);
                    Add("Content", Localize("Line endings"), DescribeLineEndings(head));
                }
                Add("Content", Localize("First bytes"), string.Join(' ', head.Take(16).Select(b => b.ToString("X2"))));
            }
            else
            {
                Add("Content", Localize("Looks like"), Localize("Empty file"));
            }

            token.ThrowIfCancellationRequested();

            // Version resource (programs, libraries).
            try
            {
                var version = FileVersionInfo.GetVersionInfo(path);
                if (!string.IsNullOrEmpty(version.FileVersion) || !string.IsNullOrEmpty(version.ProductName) || !string.IsNullOrEmpty(version.FileDescription))
                {
                    Add("Version", Localize("File description"), version.FileDescription);
                    Add("Version", Localize("File version"), version.FileVersion);
                    Add("Version", Localize("Product name"), version.ProductName);
                    Add("Version", Localize("Product version"), version.ProductVersion);
                    Add("Version", Localize("Company"), version.CompanyName);
                    Add("Version", Localize("Copyright"), version.LegalCopyright);
                    Add("Version", Localize("Original file name"), version.OriginalFilename);
                    Add("Version", Localize("Internal name"), version.InternalName);
                    Add("Version", Localize("Language"), version.Language);
                    Add("Version", Localize("Debug build"), YesNo(version.IsDebug));
                    Add("Version", Localize("Pre-release"), YesNo(version.IsPreRelease));
                }
            }
            catch (Exception ex) when (ex is IOException or FileNotFoundException or UnauthorizedAccessException)
            {
                // No version resource to show.
            }

            AddImage(path, info.Extension, (name, value) => Add("Image", name, value));

            token.ThrowIfCancellationRequested();

            // Security.
            try
            {
                var owner = info.GetAccessControl(AccessControlSections.Owner).GetOwner(typeof(NTAccount));
                Add("Security", Localize("Owner"), owner?.ToString());
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException or PlatformNotSupportedException or IdentityNotMappedException)
            {
                // Not readable.
            }

            ReadZone(path, (name, value) => Add("Security", name, value));

            // Drive.
            try
            {
                var root = Path.GetPathRoot(info.FullName);
                if (!string.IsNullOrEmpty(root))
                {
                    var drive = new DriveInfo(root);
                    if (drive.IsReady)
                    {
                        Add("Drive", Localize("Drive"), root);
                        Add("Drive", Localize("Label"), drive.VolumeLabel);
                        Add("Drive", Localize("File system"), drive.DriveFormat);
                        Add("Drive", Localize("Drive type"), drive.DriveType.ToString());
                        Add("Drive", Localize("Free space"), $"{Format.FileSize(drive.AvailableFreeSpace)} {Localize("of")} {Format.FileSize(drive.TotalSize)}");
                    }
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException)
            {
                // Not available (a network share that is gone, for instance).
            }

            return items;
        }

        #endregion

        #region Whole file.

        /// <summary>
        /// Reads the whole file once: checksums, and for text the number of lines.
        /// </summary>
        public static List<PropertyItem> Deep(SearchResult result, CancellationToken token)
        {
            var items = new List<PropertyItem>();
            var path = result.FullPath;

            try
            {
                using var stream = new FileStream(path, new FileStreamOptions
                {
                    Mode = FileMode.Open,
                    Access = FileAccess.Read,
                    Share = FileShare.ReadWrite | FileShare.Delete,
                    Options = FileOptions.SequentialScan,
                    BufferSize = 0
                });

                if (stream.Length > MaxHashSize)
                {
                    items.Add(new PropertyItem("Checksums", Localize("Checksums"), Localize("Not calculated: the file is larger than {0}.", Format.FileSize(MaxHashSize))));
                    return items;
                }

                using var md5 = IncrementalHash.CreateHash(HashAlgorithmName.MD5);
                using var sha1 = IncrementalHash.CreateHash(HashAlgorithmName.SHA1);
                using var sha256 = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

                var buffer = new byte[1024 * 1024];
                long lines = 0;
                bool endsWithNewLine = true;
                bool sawNul = false;
                int read;
                long total = 0;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    token.ThrowIfCancellationRequested();

                    var block = buffer.AsSpan(0, read);
                    md5.AppendData(block);
                    sha1.AppendData(block);
                    sha256.AppendData(block);

                    if (total == 0 && read > 0)
                    {
                        sawNul = block[..Math.Min(read, 8192)].Contains((byte)0);
                    }
                    lines += block.Count((byte)'\n');
                    endsWithNewLine = block[^1] == (byte)'\n';
                    total += read;
                }

                if (total > 0 && !sawNul)
                {
                    if (!endsWithNewLine)
                    {
                        lines++;
                    }
                    items.Add(new PropertyItem("Content", Localize("Lines"), lines.ToString("N0")));
                }

                items.Add(new PropertyItem("Checksums", "MD5", Convert.ToHexString(md5.GetHashAndReset())));
                items.Add(new PropertyItem("Checksums", "SHA-1", Convert.ToHexString(sha1.GetHashAndReset())));
                items.Add(new PropertyItem("Checksums", "SHA-256", Convert.ToHexString(sha256.GetHashAndReset())));
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                items.Add(new PropertyItem("Checksums", Localize("Checksums"), Localize("Not calculated: {0}", ex.Message)));
            }

            return items;
        }

        #endregion

        #region Pieces.

        private static string YesNo(bool value) => value ? Localize("Yes") : Localize("No");

        private static string FormatDate(DateTime date) => date.ToString("yyyy-MM-dd HH:mm:ss");

        private static string FormatAge(TimeSpan age)
        {
            if (age.TotalSeconds < 0)
            {
                return Localize("In the future");
            }
            if (age.TotalDays >= 365)
            {
                return Localize("{0} years, {1} days", (int)(age.TotalDays / 365), (int)(age.TotalDays % 365));
            }
            if (age.TotalDays >= 1)
            {
                return Localize("{0} days", (int)age.TotalDays);
            }
            if (age.TotalHours >= 1)
            {
                return Localize("{0} hours", (int)age.TotalHours);
            }
            return Localize("{0} minutes", (int)age.TotalMinutes);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern uint GetCompressedFileSizeW(string fileName, out uint high);

        private static long? SizeOnDisk(string path)
        {
            uint low = GetCompressedFileSizeW(path, out var high);
            if (low == 0xFFFFFFFF && Marshal.GetLastWin32Error() != 0)
            {
                return null;
            }
            return ((long)high << 32) | low;
        }

        private static byte[] ReadHead(string path, int length)
        {
            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                var buffer = new byte[(int)Math.Min(stream.Length, length)];
                int total = 0;
                while (total < buffer.Length)
                {
                    int read = stream.Read(buffer, total, buffer.Length - total);
                    if (read <= 0)
                    {
                        break;
                    }
                    total += read;
                }
                return total == buffer.Length ? buffer : buffer[..total];
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                return [];
            }
        }

        private static string DescribeText(byte[] head, out bool isText)
        {
            if (head.Length >= 3 && head[0] == 0xEF && head[1] == 0xBB && head[2] == 0xBF)
            {
                isText = true;
                return "UTF-8 (BOM)";
            }
            if (head.Length >= 4 && head[0] == 0xFF && head[1] == 0xFE && head[2] == 0 && head[3] == 0)
            {
                isText = true;
                return "UTF-32 LE (BOM)";
            }
            if (head.Length >= 2 && head[0] == 0xFF && head[1] == 0xFE)
            {
                isText = true;
                return "UTF-16 LE (BOM)";
            }
            if (head.Length >= 2 && head[0] == 0xFE && head[1] == 0xFF)
            {
                isText = true;
                return "UTF-16 BE (BOM)";
            }

            isText = !head.AsSpan(0, Math.Min(head.Length, 8192)).Contains((byte)0);
            if (!isText)
            {
                return string.Empty;
            }

            if (head.All(b => b < 0x80))
            {
                return "ASCII";
            }

            // The sample may end in the middle of a character.
            for (int cut = 0; cut <= 3 && cut < head.Length; cut++)
            {
                if (Utf8.IsValid(head.AsSpan(0, head.Length - cut)))
                {
                    return "UTF-8";
                }
            }

            return Localize("8-bit text (a Windows code page)");
        }

        private static string DescribeLineEndings(byte[] head)
        {
            int crlf = 0, lf = 0, cr = 0;
            for (int i = 0; i < head.Length; i++)
            {
                if (head[i] == (byte)'\r')
                {
                    if (i + 1 < head.Length && head[i + 1] == (byte)'\n')
                    {
                        crlf++;
                        i++;
                    }
                    else
                    {
                        cr++;
                    }
                }
                else if (head[i] == (byte)'\n')
                {
                    lf++;
                }
            }

            int kinds = (crlf > 0 ? 1 : 0) + (lf > 0 ? 1 : 0) + (cr > 0 ? 1 : 0);
            if (kinds == 0)
            {
                return Localize("None found");
            }
            if (kinds > 1)
            {
                return Localize("Mixed");
            }
            return crlf > 0 ? "CRLF (Windows)" : lf > 0 ? "LF (Unix)" : "CR (classic Mac)";
        }

        private static readonly (byte[] Signature, int Offset, string Name)[] _signatures =
        [
            ([0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A], 0, "PNG image"),
            ([0xFF, 0xD8, 0xFF], 0, "JPEG image"),
            ([0x47, 0x49, 0x46, 0x38], 0, "GIF image"),
            ([0x42, 0x4D], 0, "BMP image"),
            ([0x49, 0x49, 0x2A, 0x00], 0, "TIFF image"),
            ([0x4D, 0x4D, 0x00, 0x2A], 0, "TIFF image"),
            ([0x00, 0x00, 0x01, 0x00], 0, "Windows icon"),
            ([0x25, 0x50, 0x44, 0x46], 0, "PDF document"),
            ([0x50, 0x4B, 0x03, 0x04], 0, "ZIP archive (or an Office / Java package)"),
            ([0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C], 0, "7-Zip archive"),
            ([0x52, 0x61, 0x72, 0x21], 0, "RAR archive"),
            ([0x1F, 0x8B], 0, "GZIP compressed data"),
            ([0x4D, 0x5A], 0, "Windows program or library (PE)"),
            ([0x7F, 0x45, 0x4C, 0x46], 0, "ELF executable"),
            ([0x53, 0x51, 0x4C, 0x69, 0x74, 0x65, 0x20, 0x66], 0, "SQLite database"),
            ([0x7B, 0x5C, 0x72, 0x74, 0x66], 0, "Rich Text document"),
            ([0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1], 0, "Microsoft Compound File (old Office document)"),
            ([0x49, 0x44, 0x33], 0, "MP3 audio (ID3 tag)"),
            ([0x4F, 0x67, 0x67, 0x53], 0, "Ogg media"),
            ([0x66, 0x4C, 0x61, 0x43], 0, "FLAC audio"),
            ([0x66, 0x74, 0x79, 0x70], 4, "MP4 / QuickTime media"),
            ([0x1A, 0x45, 0xDF, 0xA3], 0, "Matroska / WebM media"),
            ([0x3C, 0x3F, 0x78, 0x6D, 0x6C], 0, "XML document"),
            ([0x4D, 0x53, 0x43, 0x46], 0, "Cabinet archive"),
        ];

        private static string DetectFormat(byte[] head, string extension)
        {
            if (head.Length >= 12 && head.AsSpan(0, 4).SequenceEqual("RIFF"u8))
            {
                var kind = Encoding.ASCII.GetString(head, 8, 4);
                return kind switch
                {
                    "WAVE" => Localize("WAV audio"),
                    "WEBP" => Localize("WebP image"),
                    "AVI " => Localize("AVI video"),
                    _ => Localize("RIFF container ({0})", kind)
                };
            }

            foreach (var (signature, offset, name) in _signatures)
            {
                if (head.Length >= offset + signature.Length && head.AsSpan(offset, signature.Length).SequenceEqual(signature))
                {
                    return Localize(name);
                }
            }

            return Localize("Not recognised from the file's beginning");
        }

        private static readonly HashSet<string> _imageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".bmp", ".png", ".jpg", ".jpeg", ".jpe", ".gif", ".tif", ".tiff", ".ico", ".emf", ".wmf"
        };

        private static void AddImage(string path, string extension, Action<string, string> add)
        {
            if (!_imageExtensions.Contains(extension))
            {
                return;
            }

            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                using var image = Image.FromStream(stream, useEmbeddedColorManagement: false, validateImageData: false);

                add(Localize("Dimensions"), $"{image.Width} × {image.Height} {Localize("pixels")}");
                add(Localize("Resolution"), $"{image.HorizontalResolution:0.#} × {image.VerticalResolution:0.#} DPI");
                add(Localize("Pixel format"), image.PixelFormat.ToString());
                add(Localize("Bits per pixel"), Image.GetPixelFormatSize(image.PixelFormat).ToString());
                add(Localize("Has transparency"), YesNo(Image.IsAlphaPixelFormat(image.PixelFormat)));

                try
                {
                    var dimension = new FrameDimension(image.FrameDimensionsList[0]);
                    int frames = image.GetFrameCount(dimension);
                    if (frames > 1)
                    {
                        add(Localize("Frames"), frames.ToString("N0"));
                    }
                }
                catch (Exception ex) when (ex is ArgumentException or ExternalException or IndexOutOfRangeException)
                {
                    // No frame information.
                }

                string? Text(int id)
                {
                    return image.PropertyIdList.Contains(id)
                        ? Encoding.ASCII.GetString(image.GetPropertyItem(id)!.Value ?? []).TrimEnd('\0', ' ')
                        : null;
                }

                double? Rational(int id)
                {
                    if (!image.PropertyIdList.Contains(id))
                    {
                        return null;
                    }
                    var value = image.GetPropertyItem(id)!.Value;
                    if (value == null || value.Length < 8)
                    {
                        return null;
                    }
                    uint numerator = BitConverter.ToUInt32(value, 0), denominator = BitConverter.ToUInt32(value, 4);
                    return denominator == 0 ? null : (double)numerator / denominator;
                }

                void Maybe(string name, string? value)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        add(name, value);
                    }
                }

                Maybe(Localize("Camera make"), Text(0x010F));
                Maybe(Localize("Camera model"), Text(0x0110));
                Maybe(Localize("Software"), Text(0x0131));
                Maybe(Localize("Date taken"), Text(0x9003));
                if (Rational(0x829A) is { } exposure)
                {
                    Maybe(Localize("Exposure time"), exposure >= 1 ? $"{exposure:0.#} s" : $"1/{Math.Round(1 / exposure)} s");
                }
                if (Rational(0x829D) is { } aperture)
                {
                    Maybe(Localize("Aperture"), $"f/{aperture:0.#}");
                }
                if (image.PropertyIdList.Contains(0x8827) && image.GetPropertyItem(0x8827)!.Value is { Length: >= 2 } iso)
                {
                    Maybe("ISO", BitConverter.ToUInt16(iso, 0).ToString());
                }
                if (Rational(0x920A) is { } focal)
                {
                    Maybe(Localize("Focal length"), $"{focal:0.#} mm");
                }
            }
            catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException or IOException or ExternalException or UnauthorizedAccessException)
            {
                add(Localize("Image"), Localize("The image could not be read."));
            }
        }

        /// <summary>
        /// Files that came from the internet carry a "Zone.Identifier" stream, which says where from.
        /// </summary>
        private static void ReadZone(string path, Action<string, string> add)
        {
            try
            {
                var text = File.ReadAllText(path + ":Zone.Identifier");
                foreach (var line in text.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var parts = line.Split('=', 2);
                    if (parts.Length != 2)
                    {
                        continue;
                    }

                    switch (parts[0])
                    {
                        case "ZoneId":
                            add(Localize("Security zone"), parts[1] switch
                            {
                                "0" => Localize("My computer"),
                                "1" => Localize("Local intranet"),
                                "2" => Localize("Trusted sites"),
                                "3" => Localize("Internet (downloaded)"),
                                "4" => Localize("Restricted sites"),
                                _ => parts[1]
                            });
                            break;
                        case "HostUrl":
                            add(Localize("Downloaded from"), parts[1]);
                            break;
                        case "ReferrerUrl":
                            add(Localize("Referrer"), parts[1]);
                            break;
                    }
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or NotSupportedException or ArgumentException)
            {
                // No such stream, or a file system without them.
            }
        }

        #endregion
    }
}
