namespace Phundus.Core.IdentityAndAccess.Users.Exceptions
{
    using System;
    using System.Runtime.Serialization;

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