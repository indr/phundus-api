namespace Phundus.Common
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class StaleDataException : Exception
    {
        public StaleDataException()
        {
        }


        public StaleDataException(string message) : base(message)
        {
        }

        public StaleDataException(string format, object arg0) : base(String.Format(format, arg0))
        {
        }

        public StaleDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StaleDataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}