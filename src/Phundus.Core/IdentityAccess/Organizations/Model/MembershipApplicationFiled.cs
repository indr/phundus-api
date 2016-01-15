namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class MembershipApplicationFiled : DomainEvent
    {
        public MembershipApplicationFiled(UserGuid initiator, Guid organizationGuid, UserGuid user)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (organizationGuid == null) throw new ArgumentNullException("organizationGuid");
            if (user == null) throw new ArgumentNullException("user");

            InitiatorGuid = initiator.Id;
            OrganizationGuid = organizationGuid;
            UserGuid = user.Id;
        }

        protected MembershipApplicationFiled()
        {
        }

        [Obsolete]
        public MembershipApplicationFiled(Guid initiator, int organizationGuid)
        {
            throw new NotImplementedException();
        }

        [DataMember(Order = 1)]
        public Guid InitiatorGuid { get; protected set; }

        [DataMember(Order = 2)]
        public Guid OrganizationGuid { get; protected set; }

        [DataMember(Order = 3)]
        public Guid UserGuid { get; protected set; }

        [Obsolete]
        public int UserId { get; private set; }
        [Obsolete]
        public Guid OrganizationId { get; private set; }
    }
}