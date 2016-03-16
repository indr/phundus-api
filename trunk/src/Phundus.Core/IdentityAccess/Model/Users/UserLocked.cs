namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using IdentityAccess.Model.Users;

    [DataContract]
    public class UserLocked : DomainEvent
    {
        public UserLocked(Admin initiator, UserId userId, DateTime lockedAtUtc)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (userId == null) throw new ArgumentNullException("userId");

            InitiatorId = initiator.UserId.Id;
            UserId = userId.Id;
            LockedAtUtc = lockedAtUtc;
        }

        protected UserLocked()
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