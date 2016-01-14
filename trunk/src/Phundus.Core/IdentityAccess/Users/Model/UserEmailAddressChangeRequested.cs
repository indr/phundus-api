namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserEmailAddressChangeRequested : DomainEvent
    {
        public UserEmailAddressChangeRequested(Guid initiatorGuid, User user)
        {
            if (initiatorGuid == null) throw new ArgumentNullException("initiatorGuid");
            if (user == null) throw new ArgumentNullException("user");
            InitiatorGuid = initiatorGuid;
            UserGuid = user.Guid;
        }

        protected UserEmailAddressChangeRequested()
        {
        }

        [DataMember(Order = 1)]
        public Guid InitiatorGuid { get; protected set; }

        [DataMember(Order = 2)]
        public Guid UserGuid { get; protected set; }
    }
}