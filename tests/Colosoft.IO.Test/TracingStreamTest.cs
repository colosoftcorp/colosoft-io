using System.Text;

namespace Colosoft.IO.Test
{
    public class TracingStreamTest
    {
        [Fact]
        public void GivenStreamThenTraceReadContent()
        {
            var sourceText = new StringBuilder();

            for (var i = 0; i < 100; i++)
            {
                sourceText.Append("Test");
            }

            var sourceStream = new MemoryStream(Encoding.Default.GetBytes(sourceText.ToString()));

            var totalRead = 0;
            var traceReadBytes = 0;
            using (var tracingStream = new TracingStream(sourceStream, true, false))
            {
                tracingStream.BytesRead += (_, e) =>
                {
                    traceReadBytes += e.Read;
                };

                var buffer = new byte[128];
                var read = 0;

                while ((read = tracingStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    totalRead += read;
                }
            }

            Assert.True(totalRead > 0);
            Assert.True(traceReadBytes > 0);
            Assert.Equal(totalRead, traceReadBytes);
        }
    }
}