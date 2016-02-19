namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class MembershipApplicationFiled : DomainEvent
    {
        public MembershipApplicationFiled(UserId initiator, OrganizationId organizationId, UserId user)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            if (user == null) throw new ArgumentNullException("user");

            InitiatorId = initiator.Id;
            OrganizationGuid = organizationId.Id;
            UserGuid = user.Id;
        }

        protected MembershipApplicationFiled()
        {
        }

        [DataMember(Order = 4)]
        public Guid InitiatorId { get; protected set; }

        [DataMember(Order = 3)]
        public Guid OrganizationGuid { get; protected set; }

        [DataMember(Order = 5)]
        public Guid UserGuid { get; protected set; }

        [Obsolete]
        [DataMember(Order = 1)]
        public int OrganizationShortId { get; private set; }

        [Obsolete]
        [DataMember(Order = 2)]
        public int UserShortId { get; private set; }
    }
}