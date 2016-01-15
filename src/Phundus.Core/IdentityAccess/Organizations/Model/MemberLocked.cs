namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class MemberLocked : DomainEvent
    {
        public MemberLocked(Guid organizationId, Guid memberId)
        {
            OrganizationId = organizationId;
            MemberId = memberId;
        }

        protected MemberLocked()
        {
        }

        [DataMember(Order = 1)]
        public Guid OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public Guid MemberId { get; protected set; }
    }
}