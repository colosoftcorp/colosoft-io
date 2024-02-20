using System;
using System.Collections.Generic;
using System.Linq;

namespace Colosoft.IO.FileRepository.Local
{
    public class LocalRepository : IFileRepository
    {
        public event EventHandler Updated;

        public virtual string Root { get; }

        public LocalRepository(string root)
        {
            this.Root = root ?? throw new ArgumentNullException(nameof(root));
        }

        protected LocalRepository()
        {
        }

        public void OnUpdated()
        {
            this.Updated?.Invoke(this, EventArgs.Empty);
        }

        private string GetLocalPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
            }

            path = path.Replace('/', '\\');

            if (path == "\\")
            {
                return this.Root;
            }

            if (path.StartsWith("\\", StringComparison.Ordinal))
            {
                path = path.Substring(1);
            }

            return System.IO.Path.Combine(this.Root, path);
        }

        public IEnumerable<IItem> QueryItems(string path, string searchPattern, ItemType itemType, SearchOption searchOptions)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
            }

            path = this.GetLocalPath(path);

            IEnumerable<IItem> directories = null;
            IEnumerable<IItem> files = null;

            if (string.IsNullOrEmpty(searchPattern))
            {
                searchPattern = "*.*";
            }

            if ((itemType == ItemType.Folder || itemType == ItemType.Any) && System.IO.Directory.Exists(path))
            {
                directories = System.IO.Directory.EnumerateDirectories(
                    path,
                    searchPattern,
                    searchOptions == SearchOption.AllDirectories ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly)
                    .Select(f => ((IItem)new LocalItem(new System.IO.DirectoryInfo(f))));
            }
            else
            {
                directories = Array.Empty<IItem>();
            }

            if ((itemType == ItemType.File || itemType == ItemType.Any) && System.IO.Directory.Exists(path))
            {
                files = System.IO.Directory.EnumerateFiles(
                    path,
                    searchPattern,
                    searchOptions == SearchOption.AllDirectories ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly)
                   .Select(f => ((IItem)new LocalItem(new System.IO.FileInfo(f))));
            }
            else
            {
                files = Array.Empty<IItem>();
            }

            return directories.Concat(files);
        }

        public IItem GetItem(string path, ItemType itemType)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
            }

            path = this.GetLocalPath(path);

            if (itemType == ItemType.Folder)
            {
                return new LocalItem(new System.IO.DirectoryInfo(path));
            }
            else
            {
                return new LocalItem(new System.IO.FileInfo(path));
            }
        }

        public IItem CreateFolder(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
            }

            path = this.GetLocalPath(path);
            this.OnUpdated();
            return new LocalItem(System.IO.Directory.CreateDirectory(path));
        }

        public System.IO.Stream CreateFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
            }

            path = this.GetLocalPath(path);

            var directory = System.IO.Path.GetDirectoryName(path);

            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            return System.IO.File.Create(path);
        }

        public bool Exists(string path, ItemType itemType)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
            }

            path = this.GetLocalPath(path);

            if (itemType == ItemType.Folder)
            {
                return System.IO.Directory.Exists(path);
            }
            else
            {
                return System.IO.File.Exists(path);
            }
        }

        public void Delete(IItem item, bool recursive)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var path = this.GetLocalPath(item.FullName);

            if (item.Type == ItemType.Folder)
            {
                System.IO.Directory.Delete(path, recursive);
            }
            else
            {
                System.IO.File.Delete(path);
            }

            this.OnUpdated();
        }

        public void Move(IItem sourceItem, string destPath)
        {
            if (sourceItem is null)
            {
                throw new ArgumentNullException(nameof(sourceItem));
            }

            if (string.IsNullOrEmpty(destPath))
            {
                throw new ArgumentException($"'{nameof(destPath)}' cannot be null or empty.", nameof(destPath));
            }

            var sourcePath = this.GetLocalPath(sourceItem.FullName);
            destPath = this.GetLocalPath(destPath);

            if (sourceItem.Type == ItemType.Folder)
            {
                System.IO.Directory.Move(sourcePath, destPath);
            }
            else
            {
                System.IO.File.Move(sourcePath, destPath);
            }

            this.OnUpdated();
        }

        public void Refresh()
        {
            this.OnUpdated();
        }
    }
}
