namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class MembershipApplicationRejected : DomainEvent
    {
        public MembershipApplicationRejected(UserGuid initiator, OrganizationGuid organizationGuid, UserGuid user)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (organizationGuid == null) throw new ArgumentNullException("organizationGuid");
            if (user == null) throw new ArgumentNullException("user");

            InitiatorGuid = initiator.Id;
            OrganizationGuid = organizationGuid.Id;
            UserGuid = user.Id;
        }

        protected MembershipApplicationRejected()
        {
        }

        [DataMember(Order = 1)]
        public Guid InitiatorGuid { get; protected set; }

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