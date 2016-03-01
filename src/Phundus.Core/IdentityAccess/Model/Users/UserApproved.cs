namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserApproved : DomainEvent
    {
        public UserApproved(UserId initiator, UserId user)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (user == null) throw new ArgumentNullException("user");

            InitiatorId = initiator.Id;
            UserGuid = user.Id;
        }

        protected UserApproved()
        {
        }

        [DataMember(Order = 1)]
        public Guid InitiatorId { get; protected set; }

        [DataMember(Order = 2)]
        public Guid UserGuid { get; protected set; }
    }
}