namespace Phundus.Core.Services
{
    using phiNdus.fundus.Domain.Repositories;

    public interface IMembershipService
    {
        void JoinMemberToOrganization(int memberId, int organizationId);
    }

    public class MembershipService : IMembershipService
    {
        public IMemberRepository Members { get; set; }

        public IOrganizationRepository Organizations { get; set; }


        public void JoinMemberToOrganization(int memberId, int organizationId)
        {
            var member = Members.FindById(memberId);

            var organization = Organizations.FindById(organizationId);

            member.Join(organization);
        }
    }
}