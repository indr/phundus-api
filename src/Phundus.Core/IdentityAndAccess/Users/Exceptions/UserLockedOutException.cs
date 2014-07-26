namespace Phundus.Core.IdentityAndAccess.Users.Exceptions
{
    using System;
    using System.Runtime.Serialization;

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