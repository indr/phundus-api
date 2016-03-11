namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserUnlocked : DomainEvent
    {
        public UserUnlocked(Initiator initiator, UserId userId, DateTime lockedAtUtc)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (userId == null) throw new ArgumentNullException("userId");

            InitiatorId = initiator.InitiatorId.Id;
            UserId = userId.Id;
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