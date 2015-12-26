namespace Phundus.Rest.Api
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("api/organizations")]
    public class OrganizationsController : ApiControllerBase
    {
        public IOrganizationQueries OrganizationQueries { get; set; }

        [GET("")]
        [Transaction]
        [AllowAnonymous]
        public virtual IEnumerable<OrganizationDto> Get()
        {
            return OrganizationQueries.AllNonFree();
        }


        [GET("{organizationId}")]
        [Transaction]
        [AllowAnonymous]
        public virtual OrganizationDetailDto Get(int organizationId)
        {
            var result = OrganizationQueries.ById(organizationId);
            if (result == null)
                throw new HttpException((int) HttpStatusCode.NotFound, "Organization not found.");

            return result;
        }

        [POST("")]
        [Transaction]
        [Authorize(Roles = "Admin")]
        public virtual HttpResponseMessage Post(OrganizationsPostRequestContent requestContent)
        {
            var command = new EstablishOrganization
            {
                InitiatorId = CurrentUserId,
                Name = requestContent.Name
            };
            Dispatch(command);

            return Request.CreateResponse(HttpStatusCode.OK,
                new OrganizationsPostOkResponseContent {OrganizationId = command.OrganizationId});
        }

        [PUT("{organizationId}")]
        [Transaction]
        public virtual OrganizationDetailDto Put(int organizationId, [FromBody] OrganizationDetailDto value)
        {
            Dispatcher.Dispatch(new UpdateOrganizationDetails
            {
                Address = value.Address,
                Coordinate = value.Coordinate,
                DocumentTemplate = value.DocumentTemplate,
                EmailAddress = value.EmailAddress,
                InitiatorId = CurrentUserId,
                OrganizationId = organizationId,
                Startpage = value.Startpage,
                Website = value.Website
            });

            return OrganizationQueries.ById(organizationId);
        }
    }

    public class OrganizationsPostOkResponseContent
    {
        [JsonProperty("organizationId")]
        public int OrganizationId { get; set; }
    }

    public class OrganizationsPostRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}