namespace Phundus.Core.IdentityAndAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserLocked : DomainEvent
    {
        public UserLocked(User initiator, User user, DateTime lockedAtUtc)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (user == null) throw new ArgumentNullException("user");
        }

        protected UserLocked()
        {
        }
    }
}