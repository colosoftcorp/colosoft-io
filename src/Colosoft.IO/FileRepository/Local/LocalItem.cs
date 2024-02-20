using System;

namespace Colosoft.IO.FileRepository.Local
{
    public class LocalItem : IItem
    {
        private System.IO.FileSystemInfo info;

        public string Name
        {
            get { return this.info.Name; }
        }

        public string FullName
        {
            get { return this.info.FullName; }
        }

        public long ContentLength
        {
            get
            {
                if (this.info is System.IO.FileInfo fileInfo)
                {
                    return fileInfo.Length;
                }

                return 0;
            }
        }

        public ItemType Type
        {
            get { return this.info is System.IO.FileInfo ? ItemType.File : ItemType.Folder; }
        }

        public DateTimeOffset CreationTime
        {
            get { return this.info == null ? DateTimeOffset.Now : this.info.CreationTime; }
        }

        public DateTimeOffset LastWriteTime
        {
            get { return this.info == null ? DateTimeOffset.Now : this.info.LastWriteTime; }
        }

        public bool CanRead
        {
            get { return this.info is System.IO.FileInfo; }
        }

        public bool CanWrite
        {
            get { return this.info is System.IO.FileInfo && !((System.IO.FileInfo)this.info).IsReadOnly; }
        }

        public bool Exists => this.info.Exists;

        public LocalItem(System.IO.FileSystemInfo info)
        {
            this.info = info ?? throw new ArgumentNullException(nameof(info));
        }

        public System.IO.Stream OpenRead()
        {
            if (!this.CanRead)
            {
                throw new NotSupportedException();
            }

            return ((System.IO.FileInfo)this.info).OpenRead();
        }

        public System.IO.Stream OpenWrite()
        {
            if (!this.CanWrite)
            {
                throw new NotSupportedException();
            }

            return ((System.IO.FileInfo)this.info).OpenWrite();
        }

        public void Refresh()
        {
            if (this.info is System.IO.FileInfo fileInfo)
            {
                this.info = new System.IO.FileInfo(fileInfo.FullName);
            }
            else if (this.info is System.IO.DirectoryInfo directoryInfo)
            {
                this.info = new System.IO.DirectoryInfo(directoryInfo.FullName);
            }
        }

        public override string ToString()
        {
            return this.FullName;
        }
    }
}
