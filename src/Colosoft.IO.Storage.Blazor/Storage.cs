using Colosoft.IO.Storage.Blazor.Serialization;
using System.Text.Json;

namespace Colosoft.IO.Storage.Blazor
{
    public class Storage : IStorage, ISyncStorage
    {
        private readonly IStorageProvider storageProvider;
        private readonly IJsonSerializer serializer;

        public event EventHandler<ChangingEventArgs>? Changing;
        public event EventHandler<ChangedEventArgs>? Changed;

        public Storage(IStorageProvider storageProvider, IJsonSerializer serializer)
        {
            this.storageProvider = storageProvider ?? throw new ArgumentNullException(nameof(storageProvider));
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async Task SetItem<T>(string key, T data, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var e = await this.RaiseOnChangingAsync(key, data, cancellationToken);

            if (e.Cancel)
            {
                return;
            }

            var serialisedData = this.serializer.Serialize(data);
            await this.storageProvider.SetItemAsync(key, serialisedData, cancellationToken).ConfigureAwait(false);

            this.RaiseOnChanged(key, e.OldValue, data);
        }

        public async Task SetItemAsString(string key, string data, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var e = await this.RaiseOnChangingAsync(key, data, cancellationToken);

            if (e.Cancel)
            {
                return;
            }

            await this.storageProvider.SetItemAsync(key, data, cancellationToken).ConfigureAwait(false);

            this.RaiseOnChanged(key, e.OldValue, data);
        }

        public async Task<T?> GetItem<T>(string key, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var serialisedData = await this.storageProvider.GetItemAsync(key, cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(serialisedData))
            {
                return default;
            }

            try
            {
                return this.serializer.Deserialize<T>(serialisedData);
            }
            catch (JsonException e) when (e.Path == "$" && typeof(T) == typeof(string))
            {
                return (T)(object)serialisedData;
            }
        }

        public Task<string> GetItemAsString(string key, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return this.storageProvider.GetItemAsync(key, cancellationToken);
        }

        public Task RemoveItem(string key, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return this.storageProvider.RemoveItemAsync(key, cancellationToken);
        }

        public Task Clear(CancellationToken cancellationToken) => this.storageProvider.ClearAsync(cancellationToken);

        public Task<int> Length(CancellationToken cancellationToken) => this.storageProvider.LengthAsync(cancellationToken);

        public Task<string> Key(int index, CancellationToken cancellationToken) => this.storageProvider.KeyAsync(index, cancellationToken);

        public Task<IEnumerable<string>> Keys(CancellationToken cancellationToken) => this.storageProvider.KeysAsync(cancellationToken);

        public Task<bool> ContainsKey(string key, CancellationToken cancellationToken) => this.storageProvider.ContainKeyAsync(key, cancellationToken);

        public Task RemoveItems(IEnumerable<string> keys, CancellationToken cancellationToken) => this.storageProvider.RemoveItemsAsync(keys, cancellationToken);

        private async Task<ChangingEventArgs> RaiseOnChangingAsync(string key, object? data, CancellationToken cancellationToken)
        {
            var e = new ChangingEventArgs(key, await this.GetItemInternal<object>(key, cancellationToken), data);
            this.Changing?.Invoke(this, e);
            return e;
        }

        public IEnumerable<string> Keys() => this.storageProvider.Keys();

        void ISyncStorage.SetItem<T>(string key, T data)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var e = this.RaiseOnChangingSync(key, data);

            if (e.Cancel)
            {
                return;
            }

            var serialisedData = this.serializer.Serialize(data);
            this.storageProvider.SetItem(key, serialisedData);

            this.RaiseOnChanged(key, e.OldValue, data);
        }

        void ISyncStorage.SetItemAsString(string key, string data)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var e = this.RaiseOnChangingSync(key, data);

            if (e.Cancel)
            {
                return;
            }

            this.storageProvider.SetItem(key, data);

            this.RaiseOnChanged(key, e.OldValue, data);
        }

        T ISyncStorage.GetItem<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var serialisedData = this.storageProvider.GetItem(key);

            if (string.IsNullOrWhiteSpace(serialisedData))
            {
#pragma warning disable CS8603 // Possible null reference return.
                return default;
#pragma warning restore CS8603 // Possible null reference return.
            }

            try
            {
                return this.serializer.Deserialize<T>(serialisedData);
            }
            catch (JsonException e) when (e.Path == "$" && typeof(T) == typeof(string))
            {
                return (T)(object)serialisedData;
            }
        }

        string ISyncStorage.GetItemAsString(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return this.storageProvider.GetItem(key);
        }

        void ISyncStorage.RemoveItem(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            this.storageProvider.RemoveItem(key);
        }

        void ISyncStorage.RemoveItems(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            foreach (var key in keys)
            {
                this.storageProvider.RemoveItem(key);
            }
        }

        void ISyncStorage.Clear() => this.storageProvider.Clear();

        int ISyncStorage.Length() => this.storageProvider.Length();

        string ISyncStorage.Key(int index) => this.storageProvider.Key(index);

        bool ISyncStorage.ContainsKey(string key) => this.storageProvider.ContainKey(key);

        private ChangingEventArgs RaiseOnChangingSync(string key, object data)
        {
            var e = new ChangingEventArgs(key, this.GetItemInternal(key), data);
            this.Changing?.Invoke(this, e);
            return e;
        }

        private object? GetItemInternal(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var serialisedData = this.storageProvider.GetItem(key);

            if (string.IsNullOrWhiteSpace(serialisedData))
            {
                return default;
            }

            try
            {
                return this.serializer.Deserialize<object>(serialisedData);
            }
            catch (JsonException)
            {
                return serialisedData;
            }
        }

        private async Task<T?> GetItemInternal<T>(string key, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var serialisedData = await this.storageProvider.GetItemAsync(key, cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(serialisedData))
            {
                return default;
            }

            try
            {
                return this.serializer.Deserialize<T>(serialisedData);
            }
            catch (JsonException)
            {
                return (T)(object)serialisedData;
            }
        }

        private void RaiseOnChanged(string key, object? oldValue, object? data)
        {
            var e = new ChangedEventArgs(key, oldValue, data);
            this.Changed?.Invoke(this, e);
        }
    }
}
