namespace Phundus.IdentityAccess.Resources
{
    using System;
    using Castle.Transactions;
    using Common.Resources;
    using Projections;

    public interface IOrganizationsResource
    {
        OrganizationData Get(Guid organizationId);
    }

    public class OrganizationsResource : ResourceBase, IOrganizationsResource
    {
        private readonly IOrganizationQueries _organizationQueries;

        public OrganizationsResource(IOrganizationQueries organizationQueries)
        {
            _organizationQueries = organizationQueries;
        }

        [Transaction]
        public OrganizationData Get(Guid organizationId)
        {
            return _organizationQueries.FindById(organizationId);
        }
    }
}