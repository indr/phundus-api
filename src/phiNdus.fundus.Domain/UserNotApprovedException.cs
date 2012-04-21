using System;
using System.Runtime.Serialization;

namespace phiNdus.fundus.Core.Domain
{
    [Serializable]
    public class UserNotApprovedException : Exception
    {
        public UserNotApprovedException()
        {
        }

        public UserNotApprovedException(string message) : base(message)
        {
        }

        public UserNotApprovedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserNotApprovedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}