namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class MembershipApplicationApproved : DomainEvent
    {
        public MembershipApplicationApproved(int organizationId, int userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int UserId { get; protected set; }
    }
}