using System;

namespace Colosoft.IO
{
    public delegate void BytesWrittenEventHandler(object sender, BytesWrittenEventArgs e);

    public class BytesWrittenEventArgs : EventArgs
    {
#pragma warning disable CA1819 // Properties should not return arrays
        public byte[] Buffer { get; }
#pragma warning restore CA1819 // Properties should not return arrays

        public int Offset { get; }

        public int Count { get; }

        public BytesWrittenEventArgs(byte[] buffer, int offset, int count)
        {
            this.Buffer = buffer;
            this.Offset = offset;
            this.Count = count;
        }
    }
}
