namespace Phundus.Core.IdentityAndAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserUnlocked : DomainEvent
    {
        public UserUnlocked(User initiator, User user, DateTime lockedAtUtc)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (user == null) throw new ArgumentNullException("user");

            InitiatorId = initiator.Guid;
            UserId = user.Guid;
            LockedAtUtc = lockedAtUtc;
        }

        protected UserUnlocked()
        {
        }

        [DataMember(Order = 1)]
        public Guid InitiatorId { get; protected set; }

        [DataMember(Order = 2)]
        public Guid UserId { get; protected set; }

        [DataMember(Order = 3)]
        public DateTime LockedAtUtc { get; protected set; }
    }
}