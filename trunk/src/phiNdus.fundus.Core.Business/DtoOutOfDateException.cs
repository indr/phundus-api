using System;
using System.Runtime.Serialization;

namespace phiNdus.fundus.Core.Business
{
    [Serializable]
    public class DtoOutOfDateException : Exception
    {
        public DtoOutOfDateException()
        {
        }

        public DtoOutOfDateException(string message) : base(message)
        {
        }

        public DtoOutOfDateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DtoOutOfDateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}