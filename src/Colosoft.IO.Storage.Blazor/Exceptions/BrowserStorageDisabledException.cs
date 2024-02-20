using System.Runtime.Serialization;

namespace Colosoft.IO.Storage.Blazor.Exceptions
{
    [Serializable]
    public class BrowserStorageDisabledException : Exception
    {
        public BrowserStorageDisabledException()
        {
        }

        public BrowserStorageDisabledException(string message)
            : base(message)
        {
        }

        public BrowserStorageDisabledException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        protected BrowserStorageDisabledException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
