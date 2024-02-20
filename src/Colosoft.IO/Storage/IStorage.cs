using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.IO.Storage
{
    public interface IStorage
    {
        Task Clear(CancellationToken cancellationToken);

        Task<T> GetItem<T>(string key, CancellationToken cancellationToken);

        Task<string> GetItemAsString(string key, CancellationToken cancellationToken);

        Task<bool> ContainsKey(string key, CancellationToken cancellationToken);

        Task RemoveItem(string key, CancellationToken cancellationToken);

        Task RemoveItems(IEnumerable<string> keys, CancellationToken cancellationToken);

        Task SetItem<T>(string key, T data, CancellationToken cancellationToken);

        Task SetItemAsString(string key, string data, CancellationToken cancellationToken);

        Task<int> Length(CancellationToken cancellationToken);

        Task<string> Key(int index, CancellationToken cancellationToken);

        event EventHandler<ChangingEventArgs> Changing;

        event EventHandler<ChangedEventArgs> Changed;
    }
}
