using System.Collections.Generic;

namespace Colosoft.IO.VirtualStorage
{
    public static partial class LocalStorage
    {
        public static string Directory
        {
            get { return System.IO.Path.Combine(IsolatedStorage.IsolatedStorage.AuthenticationContextDirectory, "VirtualStorage"); }
        }

        private static IEnumerable<Item> NavigateItems(string parentDirectory)
        {
            if (System.IO.Directory.Exists(parentDirectory))
            {
                foreach (var file in System.IO.Directory.GetFiles(parentDirectory))
                {
                    yield return new Item(parentDirectory, System.IO.Path.GetFileName(file), true);
                }

                foreach (var directory in System.IO.Directory.GetDirectories(parentDirectory))
                {
                    yield return new Item(parentDirectory, System.IO.Path.GetFileName(directory), false);

                    foreach (var i in NavigateItems(directory))
                    {
                        yield return i;
                    }
                }
            }
        }

        public static string GetLocalItemPath(string itemPath)
        {
            return System.IO.Path.Combine(Directory, itemPath);
        }

        public static IEnumerable<Item> GetItems()
        {
            foreach (var item in NavigateItems(Directory))
            {
                yield return item;
            }
        }
    }
}
