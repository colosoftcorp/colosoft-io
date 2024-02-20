using System;

namespace Colosoft.IO
{
    public delegate void CopyStreamCallback(long bytesWritten);

    public static class StreamUtil
    {
        public static long CopyStream(System.IO.Stream source, System.IO.Stream dest, byte[] buffer, CopyStreamCallback progressCallback)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dest is null)
            {
                throw new ArgumentNullException(nameof(dest));
            }

            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            System.Diagnostics.Stopwatch stopwatch = null;
            int num2;
            if (progressCallback != null)
            {
                stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
            }

            long bytesWritten = 0;
            long elapsedMilliseconds = 0;
            while ((num2 = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                dest.Write(buffer, 0, num2);
                bytesWritten += num2;
                if ((progressCallback != null) && (stopwatch.ElapsedMilliseconds > (elapsedMilliseconds + 0x3e8)))
                {
                    progressCallback(bytesWritten);
                    elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                }
            }

            if ((progressCallback != null) && (elapsedMilliseconds != 0))
            {
                progressCallback(bytesWritten);
            }

            return bytesWritten;
        }
    }
}
