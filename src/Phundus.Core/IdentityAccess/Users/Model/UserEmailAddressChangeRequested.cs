namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserEmailAddressChangeRequested : DomainEvent
    {
        public UserEmailAddressChangeRequested(Guid initiatorId, User user)
        {
            if (initiatorId == null) throw new ArgumentNullException("initiatorId");
            if (user == null) throw new ArgumentNullException("user");
            InitiatorId = initiatorId;
            UserGuid = user.Guid;
        }

        protected UserEmailAddressChangeRequested()
        {
        }

        [DataMember(Order = 1)]
        public Guid InitiatorId { get; protected set; }

        [DataMember(Order = 2)]
        public Guid UserGuid { get; protected set; }
    }
}