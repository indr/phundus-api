namespace Phundus.Core.IdentityAndAccess.Organizations.Model
{
    using Ddd;

    public class MemberUnlocked : DomainEvent
    {
        public MemberUnlocked(int organizationId, int memberId)
        {
            OrganizationId = organizationId;
            MemberId = memberId;
        }

        public int OrganizationId { get; protected set; }

        public int MemberId { get; protected set; }
    }
}