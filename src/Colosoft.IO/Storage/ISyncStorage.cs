using System;
using System.Collections.Generic;

namespace Colosoft.IO.Storage
{
    public interface ISyncStorage
    {
        void Clear();

        T GetItem<T>(string key);

        string GetItemAsString(string key);

        bool ContainsKey(string key);

        void RemoveItem(string key);

        void RemoveItems(IEnumerable<string> keys);

        void SetItem<T>(string key, T data);

        void SetItemAsString(string key, string data);

        int Length();

        string Key(int index);

        IEnumerable<string> Keys();

        event EventHandler<ChangingEventArgs> Changing;

        event EventHandler<ChangedEventArgs> Changed;
    }
}
