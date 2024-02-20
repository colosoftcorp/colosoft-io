using System;

namespace Colosoft.IO
{
    public class TracingStream : System.IO.Stream
    {
        private readonly bool traceContent;
        private byte[] dataRead;
        private byte[] dataWritten;
        private long numBytesRead;
        private long numBytesWritten;
        private System.Net.HttpWebResponse response;
        private System.IO.Stream stream;

        public event BytesWrittenEventHandler BytesWritten;
        public event BytesReadEventHandler BytesRead;

        public override bool CanRead
        {
            get { return this.stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.stream.CanWrite; }
        }

        public override long Length
        {
            get { return this.stream.Length; }
        }

        public long NumBytesRead
        {
            get { return this.numBytesRead; }
        }

        public long NumBytesWritten
        {
            get { return this.numBytesWritten; }
        }

        public override long Position
        {
            get { return this.stream.Position; }
            set { this.stream.Position = value; }
        }

        public TracingStream(System.IO.Stream stream)
            : this(stream, true)
        {
        }

        public TracingStream(System.IO.Stream stream, bool traceContent)
        {
            this.dataRead = Array.Empty<byte>();
            this.dataWritten = Array.Empty<byte>();
            this.stream = stream;
            this.traceContent = traceContent;
        }

        public TracingStream(System.Net.HttpWebResponse response, bool traceContent)
            : this(response?.GetResponseStream(), traceContent)
        {
            this.response = response;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this.stream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this.stream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return this.stream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            this.stream.EndWrite(asyncResult);
        }

        public override void Close()
        {
            if (this.response != null)
            {
                this.response.Close();
                this.response = null;
            }
            else if (this.stream == null)
            {
                return;
            }

            this.stream.Close();
            this.stream = null;
        }

        public override void Flush()
        {
            this.stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int length = this.stream.Read(buffer, offset, count);
            if (length > 0)
            {
                this.numBytesRead += length;
                if (this.traceContent)
                {
                    byte[] destinationArray = new byte[this.dataRead.Length + length];
                    Array.Copy(this.dataRead, destinationArray, this.dataRead.Length);
                    Array.Copy(buffer, offset, destinationArray, this.dataRead.Length, length);
                    this.dataRead = destinationArray;
                }

                this.OnBytesRead(buffer, offset, length);
                this.OnNumBytesRead();
            }

            return length;
        }

        public override int ReadByte()
        {
            var result = this.stream.ReadByte();

            this.OnBytesRead(new byte[] { (byte)result }, 0, 1);
            this.OnNumBytesRead();

            return result;
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            return this.stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count > 0)
            {
                this.numBytesWritten += count;
                if (this.traceContent)
                {
                    byte[] destinationArray = new byte[this.dataWritten.Length + count];
                    Array.Copy(this.dataWritten, destinationArray, this.dataWritten.Length);
                    Array.Copy(buffer, offset, destinationArray, this.dataWritten.Length, count);
                    this.dataWritten = destinationArray;
                }
            }

            this.stream.Write(buffer, offset, count);

            if (count > 0)
            {
                this.OnBytesWritten(buffer, offset, count);
                this.OnNumBytesWrittenUpdate();
            }
        }

        public override void WriteByte(byte value)
        {
            this.numBytesWritten++;
            if (this.traceContent)
            {
                byte[] destinationArray = new byte[this.dataWritten.Length + 1];
                Array.Copy(this.dataWritten, destinationArray, this.dataWritten.Length);
                destinationArray[this.dataWritten.Length] = value;
                this.dataWritten = destinationArray;
            }

            this.stream.WriteByte(value);

            this.OnBytesWritten(new byte[] { value }, 0, 1);
            this.OnNumBytesWrittenUpdate();
        }

        protected virtual void OnBytesWritten(byte[] buffer, int offset, int count)
        {
            this.BytesWritten?.Invoke(this, new BytesWrittenEventArgs(buffer, offset, count));
        }

        protected virtual void OnBytesRead(byte[] buffer, int offset, int read)
        {
            this.BytesRead?.Invoke(this, new BytesReadEventArgs(buffer, offset, read));
        }

        protected virtual void OnNumBytesWrittenUpdate()
        {
        }

        protected virtual void OnNumBytesRead()
        {
        }
    }
}
