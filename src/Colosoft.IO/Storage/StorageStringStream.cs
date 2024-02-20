using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.IO.Storage
{
    internal class StorageStringStream : Stream
    {
        private readonly IStorage storage;
        private readonly string key;
        private readonly Encoding encoding;
        private readonly MemoryStream inner;

        public StorageStringStream(
            IStorage storage,
            string key,
            Encoding encoding)
        {
            this.storage = storage;
            this.key = key;
            this.encoding = encoding;

            this.inner = new MemoryStream();
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

            _ = this.storage.SetItemAsString(this.key, this.encoding.GetString(this.inner.ToArray()), default);
            this.inner.Dispose();
        }

        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            await this.inner.FlushAsync(cancellationToken);
            await this.storage.SetItemAsString(this.key, this.encoding.GetString(this.inner.ToArray()), default);
        }
    }
}
