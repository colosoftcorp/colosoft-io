using System;
using System.Linq;

namespace Colosoft.IO.VirtualStorage
{
    public static class Storage
    {
        public static string GetFilePath(Uri uri)
        {
            if (uri is null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (uri.IsPackVirtualStorage())
            {
                var str = uri.PathAndQuery;
                var startIndex = 0;
                if (str[0] == '/')
                {
                    startIndex = 1;
                }

                var path = str.Substring(startIndex).Replace('/', '\\');

                return System.IO.Path.Combine(LocalStorage.Directory, path);
            }

            return null;
        }

        public static System.IO.Stream OpenFile(string path)
        {
            var path2 = System.IO.Path.Combine(LocalStorage.Directory, path);
            return System.IO.File.Open(path2, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
        }

        public static string[] GetFiles(string path)
        {
            return GetFiles(path, null);
        }

        public static string[] GetFiles(string path, string searchPattern)
        {
            return System.IO.Directory.GetFiles(System.IO.Path.Combine(LocalStorage.Directory, path), searchPattern)
                .Select(f => f.Substring(LocalStorage.Directory.Length))
                .ToArray();
        }

        public static string[] GetDirectories(string path)
        {
            return GetDirectories(path, null);
        }

        public static string[] GetDirectories(string path, string searchPattern)
        {
            return System.IO.Directory.GetDirectories(System.IO.Path.Combine(LocalStorage.Directory, path), searchPattern)
                .Select(f => f.Substring(LocalStorage.Directory.Length))
                .ToArray();
        }
    }
}
