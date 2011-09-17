using System;
using System.Runtime.Serialization;
using System.Security;

namespace phiNdus.fundus.Core.Domain
{
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

        [SecuritySafeCritical]
        protected PropertyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}