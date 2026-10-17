using System.Runtime.InteropServices;

namespace FinderSeeker.UI
{
    /// <summary>
    /// The small icons Explorer shows for files, found by extension (so no file is touched, and every .txt shares one icon).
    /// </summary>
    public sealed class ShellIcons : IDisposable
    {
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_SMALLICON = 0x1;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x10;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        private const uint FILE_ATTRIBUTE_DIRECTORY = 0x10;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)] public string szTypeName;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SHGetFileInfo(string path, uint attributes, ref SHFILEINFO info, uint size, uint flags);

        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr icon);

        private const string FolderKey = "\0folder";

        public ImageList Images { get; } = new() { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(16, 16) };

        private readonly Dictionary<string, int> _index = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The position in <see cref="Images"/> of the icon for a file or folder.
        /// </summary>
        public int IndexFor(string name, bool isDirectory)
        {
            var key = isDirectory ? FolderKey : Path.GetExtension(name);

            if (_index.TryGetValue(key, out var index))
            {
                return index;
            }

            index = Load(isDirectory ? "folder" : key.Length == 0 ? "file" : "file" + key, isDirectory);
            _index[key] = index;
            return index;
        }

        private int Load(string fakeName, bool isDirectory)
        {
            var info = new SHFILEINFO();
            var attributes = isDirectory ? FILE_ATTRIBUTE_DIRECTORY : FILE_ATTRIBUTE_NORMAL;

            SHGetFileInfo(fakeName, attributes, ref info, (uint)Marshal.SizeOf<SHFILEINFO>(),
                SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES);

            if (info.hIcon == IntPtr.Zero)
            {
                Images.Images.Add(new Bitmap(16, 16));
                return Images.Images.Count - 1;
            }

            try
            {
                using var icon = Icon.FromHandle(info.hIcon);
                Images.Images.Add(icon.ToBitmap());
            }
            finally
            {
                DestroyIcon(info.hIcon);
            }

            return Images.Images.Count - 1;
        }

        public void Dispose() => Images.Dispose();
    }
}
