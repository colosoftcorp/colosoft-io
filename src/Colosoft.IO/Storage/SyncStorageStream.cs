using System;
using System.IO;

namespace Colosoft.IO.Storage
{
    internal class SyncStorageStream : Stream
    {
        private readonly ISyncStorage storage;
        private readonly string key;
        private readonly MemoryStream inner = new MemoryStream();

        public SyncStorageStream(ISyncStorage storage, string key)
        {
            this.storage = storage;
            this.key = key;
        }

        public override bool CanRead => this.inner.CanRead;

        public override bool CanSeek => this.inner.CanSeek;

        public override bool CanWrite => this.inner.CanWrite;

        public override long Length => this.inner.Length;

        public override long Position
        {
            get => this.inner.Position;
            set => this.inner.Position = value;
        }

        public override void Flush() => this.inner.Flush();

        public override int Read(byte[] buffer, int offset, int count) =>
            this.inner.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) =>
            this.inner.Seek(offset, origin);

        public override void SetLength(long value) =>
            this.inner.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) =>
            this.inner.Write(buffer, offset, count);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (this.storage.ContainsKey(this.key))
            {
                this.storage.RemoveItem(this.key);
            }

            this.storage.SetItemAsString(this.key, Convert.ToBase64String(this.inner.ToArray()));
            this.inner.Dispose();
        }
    }
}
