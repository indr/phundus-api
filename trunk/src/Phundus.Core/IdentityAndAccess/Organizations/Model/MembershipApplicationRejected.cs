namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using Ddd;

    public class MembershipApplicationRejected : DomainEvent
    {
        public MembershipApplicationRejected(int organizationId, int userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }

        public int OrganizationId { get; protected set; }

        public int UserId { get; protected set; }
    }
}