namespace Phundus.Core.Services
{
    using phiNdus.fundus.Domain.Repositories;

    public class OrganizationService : IOrganizationService
    {
        public IOrganizationRepository Organizations { get; set; }

        public IMemberRepository Members { get; set; }


        public void CreateMembershipApplication(int organizationId, int userId)
        {
            var member = Members.FindById(userId);

            var organization = Organizations.FindById(organizationId);

            member.Join(organization);
        }
    }
}