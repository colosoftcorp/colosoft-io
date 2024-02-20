namespace Colosoft.IO.VirtualStorage
{
    public class Item
    {
        public string Name { get; }

        public string Parent { get; }

        public bool IsFile { get; }

        public string FullName
        {
            get { return System.IO.Path.Combine(this.Parent, this.Name); }
        }

        public Item(string parent, string name, bool isFile)
        {
            this.Parent = parent;
            this.Name = name;
            this.IsFile = isFile;
        }

        public System.IO.FileSystemInfo GetFileSystemInfo()
        {
            if (this.IsFile)
            {
                return new System.IO.FileInfo(this.FullName);
            }
            else
            {
                return new System.IO.DirectoryInfo(this.FullName);
            }
        }

        public byte[] CalculateHash()
        {
            if (this.IsFile)
            {
                return Checksum.CalculateMD5(this.FullName);
            }

            return System.Array.Empty<byte>();
        }
    }
}
