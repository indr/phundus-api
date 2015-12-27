namespace Phundus.Core.Shop.Orders.Services
{
    using System;
    using IdentityAndAccess.Queries;
    using Model;

    [Obsolete]
    public interface IOrganizationService
    {
        Organization ById(int organizationId);
    }

    public class OrganizationService : IOrganizationService
    {
        public IOrganizationQueries OrganizationQueries { get; set; }

        public Organization ById(int organizationId)
        {
            var organization = OrganizationQueries.ById(organizationId);
            if (organization == null)
                throw new OrganizationNotFoundException(organizationId);

            return ToOrganization(organization);
        }

        private static Organization ToOrganization(OrganizationDetailDto organization)
        {
            return new Organization(organization.Id, organization.Name);
        }
    }

    public class OrganizationNotFoundException : Exception
    {
        public OrganizationNotFoundException(int id)
            : base(String.Format("Organisation mit Id {0} konnte nicht gefunden werden.", id))
        {
        }
    }
}