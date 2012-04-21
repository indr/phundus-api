using System;
using System.Runtime.Serialization;

namespace phiNdus.fundus.Core.Domain
{
    [Serializable]
    public class UserLockedOutException : Exception
    {
        public UserLockedOutException()
        {
        }

        public UserLockedOutException(string message) : base(message)
        {
        }

        public UserLockedOutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserLockedOutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}