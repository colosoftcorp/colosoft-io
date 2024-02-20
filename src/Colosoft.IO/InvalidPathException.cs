using System;

namespace Colosoft.IO
{
    [Serializable]
    public class InvalidPathException : ArgumentException
    {
        public InvalidPathException(string message)
            : base(message)
        {
        }

        protected InvalidPathException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        public InvalidPathException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public InvalidPathException()
        {
        }
    }
}
