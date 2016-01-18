namespace Phundus.Rest.Api.Admin
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using ContentObjects;
    using IdentityAccess.Queries;

    [RoutePrefix("api/admin/organizations")]
    [Authorize(Roles = "Admin")]
    public class AdminOrganizationsController : ApiControllerBase
    {
        private readonly IOrganizationQueries _organizationQueries;

        public AdminOrganizationsController(IOrganizationQueries organizationQueries)
        {
            if (organizationQueries == null) throw new ArgumentNullException("organizationQueries");
            _organizationQueries = organizationQueries;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<AdminOrganization> Get()
        {
            var results = _organizationQueries.All();
            return new QueryOkResponseContent<AdminOrganization>
            {
                Results = results.Select(s => new AdminOrganization
                {
                    OrganizationId = s.OrganizationId,
                    Name = s.Name,
                    EstablishedAtUtc = s.EstablishedAtUtc
                }).ToList()
            };
        } 
    }
}