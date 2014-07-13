using System;
using System.Runtime.Serialization;

namespace phiNdus.fundus.Domain
{
    [Serializable]
    public class FieldAlreadyAttachedException : Exception
    {
        public FieldAlreadyAttachedException()
        {
        }

        public FieldAlreadyAttachedException(string message) : base(message)
        {
        }

        public FieldAlreadyAttachedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FieldAlreadyAttachedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}