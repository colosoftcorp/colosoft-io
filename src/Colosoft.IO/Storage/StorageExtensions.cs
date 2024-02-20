using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.IO.Storage
{
    public static class StorageExtensions
    {
        public static async Task<System.IO.Stream> ReadStreamFromString(
            this IStorage storage,
            string key,
            Encoding encoding,
            CancellationToken cancellationToken)
        {
            if (storage is null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var item = await storage.GetItemAsString(key, cancellationToken);

            if (string.IsNullOrEmpty(item))
            {
                return null;
            }

            return new System.IO.MemoryStream(encoding.GetBytes(item));
        }

        public static Task<System.IO.Stream> ReadStreamFromString(
            this IStorage storage,
            string key,
            CancellationToken cancellationToken) => ReadStreamFromString(storage, key, Encoding.Default, cancellationToken);

        public static System.IO.Stream CreateStreamToString(
            this IStorage storage,
            string key,
            System.Text.Encoding encoding)
        {
            if (storage is null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            return new StorageStringStream(storage, key, encoding);
        }

        public static System.IO.Stream CreateStreamToString(this IStorage storage, string key) =>
            CreateStreamToString(storage, key, Encoding.Default);

        public static System.IO.Stream ReadStream(this ISyncStorage storage, string key)
        {
            if (storage is null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var item = storage.GetItemAsString(key);

            if (string.IsNullOrEmpty(item))
            {
                return null;
            }

            return new System.IO.MemoryStream(Convert.FromBase64String(item));
        }

        public static System.IO.Stream CreateStream(
            this ISyncStorage storage,
            string key)
        {
            if (storage is null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return new SyncStorageStream(storage, key);
        }
    }
}
