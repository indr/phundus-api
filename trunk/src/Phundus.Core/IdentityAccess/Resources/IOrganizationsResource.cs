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
        private readonly IOrganizationQueryService _organizationQueryService;

        public OrganizationsResource(IOrganizationQueryService organizationQueryService)
        {
            _organizationQueryService = organizationQueryService;
        }

        [Transaction]
        public OrganizationData Get(Guid organizationId)
        {
            return _organizationQueryService.FindById(organizationId);
        }
    }
}