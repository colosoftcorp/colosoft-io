using Colosoft.IO.Storage.Blazor.Exceptions;
using Microsoft.JSInterop;

namespace Colosoft.IO.Storage.Blazor
{
    public class BrowserStorageProvider : IStorageProvider
    {
        private const string StorageNotAvailableMessage = "Unable to access the browser storage. This is most likely due to the browser settings.";

        private readonly IJSRuntime jsRuntime;
        private readonly IJSInProcessRuntime? jsInProcessRuntime;

        public BrowserStorageProvider(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
            this.jsInProcessRuntime = jsRuntime as IJSInProcessRuntime;
        }

        private static bool IsStorageDisabledException(Exception exception) =>
            exception.Message.Contains("Failed to read the 'localStorage' property from 'Window'");

        public async Task ClearAsync(CancellationToken cancellationToken)
        {
            try
            {
                await this.jsRuntime.InvokeVoidAsync("localStorage.clear", cancellationToken);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async Task<string> GetItemAsync(string key, CancellationToken cancellationToken)
        {
            try
            {
                return await this.jsRuntime.InvokeAsync<string>("localStorage.getItem", cancellationToken, key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async Task<string> KeyAsync(int index, CancellationToken cancellationToken)
        {
            try
            {
                return await this.jsRuntime.InvokeAsync<string>("localStorage.key", cancellationToken, index);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async Task<bool> ContainKeyAsync(string key, CancellationToken cancellationToken)
        {
            try
            {
                return await this.jsRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", cancellationToken, key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async Task<int> LengthAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await this.jsRuntime.InvokeAsync<int>("eval", cancellationToken, "localStorage.length");
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async Task RemoveItemAsync(string key, CancellationToken cancellationToken)
        {
            try
            {
                await this.jsRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async Task SetItemAsync(string key, string data, CancellationToken cancellationToken)
        {
            try
            {
                await this.jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, data);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async Task<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await this.jsRuntime.InvokeAsync<IEnumerable<string>>("eval", cancellationToken, "Object.keys(localStorage)");
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public async Task RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var key in keys)
                {
                    await this.jsRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
                }
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public void Clear()
        {
            this.CheckForInProcessRuntime();

            try
            {
                this.jsInProcessRuntime!.InvokeVoid("localStorage.clear");
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public string GetItem(string key)
        {
            this.CheckForInProcessRuntime();

            try
            {
                return this.jsInProcessRuntime!.Invoke<string>("localStorage.getItem", key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public string Key(int index)
        {
            this.CheckForInProcessRuntime();

            try
            {
                return this.jsInProcessRuntime!.Invoke<string>("localStorage.key", index);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public bool ContainKey(string key)
        {
            this.CheckForInProcessRuntime();

            try
            {
                return this.jsInProcessRuntime!.Invoke<bool>("localStorage.hasOwnProperty", key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public int Length()
        {
            this.CheckForInProcessRuntime();

            try
            {
                return this.jsInProcessRuntime!.Invoke<int>("eval", "localStorage.length");
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public void RemoveItem(string key)
        {
            this.CheckForInProcessRuntime();

            try
            {
                this.jsInProcessRuntime!.InvokeVoid("localStorage.removeItem", key);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public void RemoveItems(IEnumerable<string> keys)
        {
            this.CheckForInProcessRuntime();

            try
            {
                foreach (var key in keys)
                {
                    this.jsInProcessRuntime!.InvokeVoid("localStorage.removeItem", key);
                }
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public void SetItem(string key, string data)
        {
            this.CheckForInProcessRuntime();

            try
            {
                this.jsInProcessRuntime!.InvokeVoid("localStorage.setItem", key, data);
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        public IEnumerable<string> Keys()
        {
            this.CheckForInProcessRuntime();

            try
            {
                return this.jsInProcessRuntime!.Invoke<IEnumerable<string>>("eval", "Object.keys(localStorage)");
            }
            catch (Exception exception)
            {
                if (IsStorageDisabledException(exception))
                {
                    throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
                }

                throw;
            }
        }

        private void CheckForInProcessRuntime()
        {
            if (this.jsInProcessRuntime == null)
            {
                throw new InvalidOperationException("IJSInProcessRuntime not available");
            }
        }
    }
}
