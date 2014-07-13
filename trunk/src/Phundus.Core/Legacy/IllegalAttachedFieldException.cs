using System;
using System.Runtime.Serialization;

namespace phiNdus.fundus.Domain
{
    public class IllegalAttachedFieldException : Exception
    {
        public IllegalAttachedFieldException()
        {
        }

        public IllegalAttachedFieldException(string message) : base(message)
        {
        }

        public IllegalAttachedFieldException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalAttachedFieldException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}