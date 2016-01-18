namespace Phundus.IdentityAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class MemberUnlocked : DomainEvent
    {
        public MemberUnlocked(OrganizationId organizationId, Guid memberId)
        {
            OrganizationId = organizationId.Id;
            MemberId = memberId;
        }

        protected MemberUnlocked()
        {
        }

        [DataMember(Order = 1)]
        public Guid OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public Guid MemberId { get; protected set; }
    }
}