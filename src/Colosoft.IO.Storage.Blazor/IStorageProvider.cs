namespace Colosoft.IO.Storage.Blazor
{
    public interface IStorageProvider
    {
        void Clear();

        Task ClearAsync(CancellationToken cancellationToken);

        bool ContainKey(string key);

        Task<bool> ContainKeyAsync(string key, CancellationToken cancellationToken);

        string GetItem(string key);

        Task<string> GetItemAsync(string key, CancellationToken cancellationToken);

        string Key(int index);

        Task<string> KeyAsync(int index, CancellationToken cancellationToken);

        IEnumerable<string> Keys();

        Task<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken);

        int Length();

        Task<int> LengthAsync(CancellationToken cancellationToken);

        void RemoveItem(string key);

        Task RemoveItemAsync(string key, CancellationToken cancellationToken);

        void RemoveItems(IEnumerable<string> keys);

        Task RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken);

        void SetItem(string key, string data);

        Task SetItemAsync(string key, string data, CancellationToken cancellationToken);
    }
}