using System;
using System.IO;

namespace Colosoft.IO
{
    public class StreamWrapper : Stream
    {
        private Stream streamBase;

        public event EventHandler Disposed;

        public StreamWrapper(Stream streamBase)
        {
            this.streamBase = streamBase ?? throw new ArgumentNullException(nameof(streamBase));
        }

        public override bool CanRead
        {
            get { return this.streamBase != null && this.streamBase.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.streamBase != null && this.streamBase.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.streamBase != null && this.streamBase.CanWrite; }
        }

        public override long Length
        {
            get
            {
                this.ThrowIfDisposed();
                return this.streamBase.Length;
            }
        }

        public override long Position
        {
            get
            {
                this.ThrowIfDisposed();
                return this.streamBase.Position;
            }
            set
            {
                this.ThrowIfDisposed();
                this.streamBase.Position = value;
            }
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            this.ThrowIfDisposed();
            return this.streamBase.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            this.ThrowIfDisposed();
            return this.streamBase.BeginWrite(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            this.ThrowIfDisposed();
            return this.streamBase.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            this.ThrowIfDisposed();
            this.streamBase.EndWrite(asyncResult);
        }

        public override void Flush()
        {
            this.ThrowIfDisposed();
            this.streamBase.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            this.ThrowIfDisposed();
            return this.streamBase.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            this.ThrowIfDisposed();
            return this.streamBase.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            this.ThrowIfDisposed();
            return this.streamBase.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.ThrowIfDisposed();
            this.streamBase.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.ThrowIfDisposed();
            this.streamBase.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            this.ThrowIfDisposed();
            this.streamBase.WriteByte(value);
        }

        protected Stream WrappedStream
        {
            get { return this.streamBase; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.streamBase = null;
            }

            base.Dispose(disposing);

            this.Disposed?.Invoke(this, EventArgs.Empty);
        }

        private void ThrowIfDisposed()
        {
            if (this.streamBase == null)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }
    }
}
