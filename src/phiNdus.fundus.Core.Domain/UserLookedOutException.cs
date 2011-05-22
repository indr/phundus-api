using System;
using System.Runtime.Serialization;

namespace phiNdus.fundus.Core.Domain
{
    [Serializable]
    public class UserLookedOutException : Exception
    {
        public UserLookedOutException()
        {
        }

        public UserLookedOutException(string message) : base(message)
        {
        }

        public UserLookedOutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserLookedOutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}