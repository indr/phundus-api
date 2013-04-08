namespace phiNdus.fundus.Web.Controllers.WebApi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
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

        // GET api/organizations/5
        [Transaction]
        public virtual OrganizationDto Get(int id)
        {
            var result = Organizations.FindById(id);
            if (result == null)
                throw new HttpException(404, "Die Organisation ist nicht vorhanden.");

            return new OrganizationDto
                       {
                           Id = result.Id,
                           Version = result.Version,
                           Name = result.Name,
                           Url = result.Url,
                           Address = result.Address,
                           Coordinate = result.Coordinate,
                           Startpage = result.Startpage
                       };
        }
    }

    public class OrganizationDto
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Address { get; set; }
        public string Coordinate { get; set; }
        public string Startpage { get; set; }
    }
}