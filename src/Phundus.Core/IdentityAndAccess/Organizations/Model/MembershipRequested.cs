namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using Ddd;

    public class MembershipRequested : DomainEvent
    {
        public MembershipRequested(int organizationId, int userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }

        public int OrganizationId { get; private set; }

        public int UserId { get; private set; }
    }
}