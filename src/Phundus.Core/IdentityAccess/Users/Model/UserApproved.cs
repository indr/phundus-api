namespace Phundus.IdentityAccess.Users.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class UserApproved : DomainEvent
    {
        public UserApproved(UserGuid initiator, UserGuid user)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (user == null) throw new ArgumentNullException("user");

            InitiatorGuid = initiator.Id;
            UserGuid = user.Id;
        }

        protected UserApproved()
        {
        }

        [DataMember(Order = 1)]
        public Guid InitiatorGuid { get; protected set; }

        [DataMember(Order = 2)]
        public Guid UserGuid { get; protected set; }
    }
}