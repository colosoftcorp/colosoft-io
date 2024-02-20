using System;

namespace Colosoft.IO
{
    public delegate void BytesReadEventHandler(object sender, BytesReadEventArgs e);

    public class BytesReadEventArgs : EventArgs
    {
#pragma warning disable CA1819 // Properties should not return arrays
        public byte[] Buffer { get; }
#pragma warning restore CA1819 // Properties should not return arrays

        public int Offset { get; }

        public int Read { get; }

        public BytesReadEventArgs(byte[] buffer, int offset, int read)
        {
            this.Buffer = buffer;
            this.Offset = offset;
            this.Read = read;
        }
    }
}
