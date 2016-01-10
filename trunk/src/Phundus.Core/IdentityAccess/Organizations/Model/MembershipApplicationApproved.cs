namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class MembershipApplicationApproved : DomainEvent
    {
        public MembershipApplicationApproved(Guid organizationId, int userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }

        protected MembershipApplicationApproved()
        {
        }

        [DataMember(Order = 1)]
        public Guid OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int UserId { get; protected set; }
    }
}