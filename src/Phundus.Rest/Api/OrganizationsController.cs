namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using ContentObjects;
    using IdentityAccess.Organizations.Commands;
    using IdentityAccess.Queries;
    using IdentityAccess.Queries.ReadModels;
    using Inventory.Queries;
    using Newtonsoft.Json;

    [RoutePrefix("api/organizations")]
    public class OrganizationsController : ApiControllerBase
    {
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IStoreQueries _storeQueries;

        public OrganizationsController(IOrganizationQueries organizationQueries, IStoreQueries storeQueries)
        {
            AssertionConcern.AssertArgumentNotNull(organizationQueries, "OrganizationQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(storeQueries, "StoreQueries must be provided.");

            _organizationQueries = organizationQueries;
            _storeQueries = storeQueries;
        }


        [GET("")]
        [Transaction]
        [AllowAnonymous]
        public virtual OrganizationsQueryOkResponseContent Get()
        {
            var result = _organizationQueries.All();
            return new OrganizationsQueryOkResponseContent
            {
                Results = Map<List<Organization>>(result)
            };
        }

        [GET("{organizationId}")]
        [Transaction]
        [AllowAnonymous]
        public virtual OrganizationsGetOkResponseContent Get(Guid organizationId)
        {
            var organization = _organizationQueries.FindById(organizationId);
            if (organization == null)
                throw new HttpException((int) HttpStatusCode.NotFound, "Organization not found.");

            var store = _storeQueries.FindByOwnerId(new OwnerId(organization.Guid));
            var result = new OrganizationsGetOkResponseContent
            {
                Address = organization.Address,
                DocumentTemplate = organization.DocumentTemplate,
                EmailAddress = organization.EmailAddress,
                Name = organization.Name,
                OrganizationId = organization.Guid,
                Startpage = organization.Startpage,
                Stores = new List<Store>(),
                Url = organization.Url,
                Website = organization.Website,
                ContactDetails = new ContactDetails
                {
                    EmailAddress = organization.EmailAddress,
                    PostAddress = organization.Address,
                    PhoneNumber = organization.PhoneNumber,
                    Website = organization.Website
                }
            };
            if (store != null)
            {
                result.Stores.Add(new Store
                {
                    Address = store.Address,
                    OpeningHours = store.OpeningHours,
                    Coordinate = Coordinate.FromLatLng(store.Latitude, store.Longitude),
                    StoreId = store.StoreId.Id
                });
            }
            return result;
        }

        [POST("")]
        [Transaction]
        [Authorize(Roles = "Admin")]
        public virtual HttpResponseMessage Post(OrganizationsPostRequestContent requestContent)
        {
            if (requestContent == null) throw new ArgumentNullException("requestContent");

            var organizationGuid = new OrganizationGuid();
            Dispatch(new EstablishOrganization(CurrentUserId, organizationGuid, requestContent.Name));

            return Request.CreateResponse(HttpStatusCode.OK,
                new OrganizationsPostOkResponseContent {OrganizationId = organizationGuid.Id});
        }

        [PATCH("{organizationId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(Guid organizationId, OrganizationsPatchRequestContent requestContent)
        {
            if (requestContent == null) throw new ArgumentNullException("requestContent");

            if (requestContent.ContactDetails != null)
            {
                Dispatch(new ChangeOrganizationContactDetails(CurrentUserId, new OrganizationGuid(organizationId),
                    requestContent.ContactDetails.PostAddress, requestContent.ContactDetails.PhoneNumber,
                    requestContent.ContactDetails.EmailAddress, requestContent.ContactDetails.Website));
            }

            if (!String.IsNullOrWhiteSpace(requestContent.Startpage))
            {
                Dispatch(new UpdateStartpage(CurrentUserId, new OrganizationGuid(organizationId), requestContent.Startpage));
            }
            return NoContent();
        }
    }

    public class OrganizationsPatchRequestContent
    {
        [JsonProperty("startpage")]
        public string Startpage { get; set; }

        [JsonProperty("contactDetails")]
        public ContactDetails ContactDetails { get; set; }
    }

    public class ContactDetails
    {
        [JsonProperty("postAddress")]
        public string PostAddress { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }
    }

    public class OrganizationsQueryOkResponseContent : QueryOkResponseContent<Organization>
    {
        
    }

    public class OrganizationsGetOkResponseContent
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("startpage")]
        public string Startpage { get; set; }

        [JsonProperty("documentTemplate")]
        public string DocumentTemplate { get; set; }

        [JsonProperty("stores")]
        public List<Store> Stores { get; set; }

        [JsonProperty("contactDetails")]
        public ContactDetails ContactDetails { get; set; }
    }

    public class OrganizationsPostOkResponseContent
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }
    }

    public class OrganizationsPostRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}