namespace phiNdus.fundus.Web.Controllers.WebApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Castle.Transactions;
    using phiNdus.fundus.Domain.Repositories;

    public class OrganizationsController : ApiController
    {
        public IOrganizationRepository Organizations { get; set; }

        [Transaction]
        public virtual IEnumerable<OrganizationDto> Get()
        {
            return Organizations
                .FindAll()
                .Select(each => new OrganizationDto
                                    {
                                        Id = each.Id,
                                        Version = each.Version,
                                        Url = each.Url
                                    })
                .ToList();
        }
    }

    public class OrganizationDto
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public string Url { get; set; }
    }
}