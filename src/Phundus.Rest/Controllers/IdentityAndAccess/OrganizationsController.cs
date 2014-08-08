namespace Phundus.Rest.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Queries;
    using Exceptions;

    
    public class OrganizationsController : ApiControllerBase
    {
        public IOrganizationQueries OrganizationQueries { get; set; }

        [Transaction]
        [AllowAnonymous]
        public virtual IEnumerable<OrganizationDto> Get()
        {
            return OrganizationQueries.All();
        }

        // GET api/organizations/5
        [Transaction]
        [AllowAnonymous]
        public virtual OrganizationDetailDto Get(int id)
        {
            var result = OrganizationQueries.ById(id);
            if (result == null)
                throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

            return result;
        }

        // PUT api/organizations/5
        [Transaction]
        public virtual OrganizationDetailDto Put(int id, [FromBody] OrganizationDetailDto value)
        {
            Dispatcher.Dispatch(new UpdateOrganizationDetails
            {
                Address = value.Address,
                Coordinate = value.Coordinate,
                DocumentTemplate = value.DocumentTemplate,
                EmailAddress = value.EmailAddress,
                InitiatorId = CurrentUserId,
                OrganizationId = id,
                Startpage = value.Startpage,
                Website = value.Website
            });


            return OrganizationQueries.ById(id);
        }
    }
}