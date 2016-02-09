namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class MemberLocked : DomainEvent
    {
        public MemberLocked(OrganizationId organizationId, Guid userGuid)
        {
            OrganizationGuid = organizationId.Id;
            UserGuid = userGuid;
        }

        protected MemberLocked()
        {
        }

        [DataMember(Order = 3)]
        public Guid OrganizationGuid { get; protected set; }

        [DataMember(Order = 4)]
        public Guid UserGuid { get; protected set; }
        
        [DataMember(Order = 5)]
        public Guid InitiatorId { get; private set; }

        [Obsolete]
        [DataMember(Order = 1)]        
        public int OrganizationShortId { get; protected set; }

        [Obsolete]
        [DataMember(Order = 2)]
        public int UserShortId { get; protected set; }
    }
}