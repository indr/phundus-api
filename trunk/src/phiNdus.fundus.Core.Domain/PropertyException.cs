using System;
using System.Runtime.Serialization;

namespace phiNdus.fundus.Core.Domain
{
    [Serializable]
    public class PropertyException : Exception
    {
        public PropertyException()
        {
        }

        public PropertyException(string message) : base(message)
        {
        }

        public PropertyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PropertyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}