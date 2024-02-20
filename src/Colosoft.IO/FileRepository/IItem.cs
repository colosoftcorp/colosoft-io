using System;

namespace Colosoft.IO.FileRepository
{
    public interface IItem
    {
        string Name { get; }

        string FullName { get; }

        long ContentLength { get; }

        ItemType Type { get; }

        DateTimeOffset CreationTime { get; }

        DateTimeOffset LastWriteTime { get; }

        bool CanRead { get; }

        bool CanWrite { get; }

        bool Exists { get; }

        System.IO.Stream OpenRead();

        System.IO.Stream OpenWrite();

        void Refresh();
    }
}
