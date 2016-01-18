namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class MembershipApplicationRejected : DomainEvent
    {
        public MembershipApplicationRejected(UserId initiator, OrganizationId organizationId, UserId user)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (user == null) throw new ArgumentNullException("user");

            InitiatorId = initiator.Id;
            OrganizationGuid = organizationId.Id;
            UserGuid = user.Id;
        }

        protected MembershipApplicationRejected()
        {
        }

        [DataMember(Order = 1)]
        public Guid InitiatorId { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrganizationGuid { get; protected set; }

        [DataMember(Order = 3)]
        public Guid UserGuid { get; protected set; }

        [Obsolete]
        public Guid OrganizationId { get; private set; }
        [Obsolete]
        public int UserId { get; private set; }
    }
}