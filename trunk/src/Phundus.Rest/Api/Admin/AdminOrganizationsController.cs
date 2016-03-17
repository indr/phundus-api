namespace Phundus.Rest.Api.Admin
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Resources;
    using ContentObjects;
    using IdentityAccess.Application;    

    [RoutePrefix("api/admin/organizations")]
    [Authorize(Roles = "Admin")]
    public class AdminOrganizationsController : ApiControllerBase
    {
        private readonly IOrganizationQueryService _organizationQueryService;

        public AdminOrganizationsController(IOrganizationQueryService organizationQueryService)
        {
            if (organizationQueryService == null) throw new ArgumentNullException("organizationQueryService");
            _organizationQueryService = organizationQueryService;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<AdminOrganization> Get()
        {
            var results = _organizationQueryService.Query();
            return new QueryOkResponseContent<AdminOrganization>
            {
                Results = results.Select(s => new AdminOrganization
                {
                    OrganizationId = s.OrganizationId,
                    Name = s.Name,
                    EstablishedAtUtc = s.EstablishedAtUtc,
                    Plan = s.Plan.ToLowerInvariant()
                }).ToList()
            };
        }
    }
}