namespace Phundus.Rest.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Castle.Transactions;
    using Core.OrganizationAndMembershipCtx.Queries;
    using Core.OrganizationAndMembershipCtx.Repositories;
    using Exceptions;

    public class OrganizationsController : ApiControllerBase
    {
        public IOrganizationRepository Organizations { get; set; }

        public IOrganizationQueries OrganizationQueries { get; set; }

        [Transaction]
        public virtual IEnumerable<OrganizationDto> Get()
        {
            return OrganizationQueries.All();
        }

        // GET api/organizations/5
        [Transaction]
        public virtual OrganizationDetailDto Get(int id)
        {
            var result = OrganizationQueries.ById(id);
            if (result == null)
                throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

            return result;
        }

        // PUT api/organizations/5
        [Transaction]
        [Authorize]
        public virtual OrganizationDetailDto Put(int id, [FromBody] OrganizationDetailDto value)
        {
            throw new NotSupportedException();
            //var org = Organizations.FindById(id);

            //var user = Users.FindByEmail(Identity.Name);

            //if (org == null)
            //    throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

            //if (user == null || !user.IsChiefOf(org))
            //    throw new HttpForbiddenException("Sie haben keine Berechtigung um die Organisation zu aktualisieren.");

            //if (org.Version != value.Version)
            //    throw new HttpConflictException("Die Organisation wurde in der Zwischenzeit verändert.");

            //org.Address = value.Address;
            //org.EmailAddress = value.EmailAddress;
            //org.Website = value.Website;
            //org.Coordinate = value.Coordinate;
            //org.Startpage = value.Startpage;
            //org.DocTemplateFileName = value.DocumentTemplate;

            //Organizations.Update(org);
            //SessionFactory().Flush();

            //return ToDto(org);
        }
    }
}