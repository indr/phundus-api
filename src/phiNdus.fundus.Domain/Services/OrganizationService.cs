namespace Phundus.Core.Services
{
    using System;
    using phiNdus.fundus.Domain.Repositories;

    public class OrganizationService : IOrganizationService
    {
        public IOrganizationRepository Organizations { get; set; }

        public IUserRepository Users { get; set; }

        [Obsolete]
        public void CreateMembershipApplication(int organizationId, int userId)
        {
            var user = Users.FindById(userId);

            var organization = Organizations.FindById(organizationId);

            user.Join(organization);
        }
    }
}