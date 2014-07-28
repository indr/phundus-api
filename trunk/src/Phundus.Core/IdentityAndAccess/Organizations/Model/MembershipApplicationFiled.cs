namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using Ddd;

    public class MembershipApplicationFiled : DomainEvent
    {
        public MembershipApplicationFiled(int organizationId, int userId)
        {
            OrganizationId = organizationId;
            UserId = userId;
        }

        public int OrganizationId { get; protected set; }

        public int UserId { get; protected set; }
    }
}