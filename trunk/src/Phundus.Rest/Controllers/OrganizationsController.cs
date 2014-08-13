namespace Phundus.Rest.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Queries;
    using Exceptions;

    [RoutePrefix("api/organizations")]
    public class OrganizationsController : ApiControllerBase
    {
        public IOrganizationQueries OrganizationQueries { get; set; }

        [GET("")]
        [Transaction]
        [AllowAnonymous]
        public virtual IEnumerable<OrganizationDto> Get()
        {
            return OrganizationQueries.All();
        }


        [GET("{organizationId}")]
        [Transaction]
        [AllowAnonymous]
        public virtual OrganizationDetailDto Get(int organizationId)
        {
            var result = OrganizationQueries.ById(organizationId);
            if (result == null)
                throw new HttpNotFoundException("Die Organisation ist nicht vorhanden.");

            return result;
        }

        [PUT("{organizationId}")]
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