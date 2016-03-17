namespace Phundus.IdentityAccess.Resources
{
    using System;
    using Application;
    using Castle.Transactions;
    using Common.Resources;

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