using System;
using System.Collections.Generic;

namespace Colosoft.IO.FileRepository
{
    public interface IFileRepository
    {
        event EventHandler Updated;

        IEnumerable<IItem> QueryItems(string path, string searchPattern, ItemType itemType, SearchOption searchOptions);

        IItem GetItem(string path, ItemType itemType);

        IItem CreateFolder(string path);

        System.IO.Stream CreateFile(string path);

        bool Exists(string path, ItemType itemType);

        void Delete(IItem item, bool recursive);

        void Move(IItem sourceItem, string destPath);

        void Refresh();
    }
}
