using System;
using System.Runtime.Serialization;

namespace phiNdus.fundus.Business
{
    [Serializable]
    public class EmailAlreadyTakenException : Exception
    {
        public EmailAlreadyTakenException()
        {
        }

        public EmailAlreadyTakenException(string message) : base(message)
        {
        }

        public EmailAlreadyTakenException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmailAlreadyTakenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}