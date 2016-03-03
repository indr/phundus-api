namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using ContentObjects;
    using IdentityAccess.Application;
    using IdentityAccess.Projections;
    using Inventory.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/organizations")]
    public class OrganizationsController : ApiControllerBase
    {
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IStoresQueries _storesQueries;

        public OrganizationsController(IOrganizationQueries organizationQueries, IStoresQueries storesQueries)
        {
            AssertionConcern.AssertArgumentNotNull(organizationQueries, "OrganizationQueries must be provided.");
            AssertionConcern.AssertArgumentNotNull(storesQueries, "StoreQueries must be provided.");

            _organizationQueries = organizationQueries;
            _storesQueries = storesQueries;
        }

        [GET("")]
        [Transaction]
        [AllowAnonymous]
        public virtual QueryOkResponseContent<Organization> Get()
        {
            var result = _organizationQueries.Query();
            return new QueryOkResponseContent<Organization>
            {
                Results = result.Select(s => new Organization
                {
                    PostalAddress = s.PostalAddress,
                    Name = s.Name,
                    OrganizationId = s.OrganizationId,
                    Url = s.Url
                }).ToList()
            };
        }

        [GET("{organizationId}")]
        [Transaction]
        [AllowAnonymous]
        public virtual OrganizationsGetOkResponseContent Get(Guid organizationId)
        {
            var organization = _organizationQueries.GetById(organizationId);

            var store = _storesQueries.FindByOwnerId(organization.OrganizationId);
            var result = new OrganizationsGetOkResponseContent
            {
                Name = organization.Name,
                OrganizationId = organization.OrganizationId,
                Startpage = organization.Startpage,
                Stores = new List<Store>(),
                Url = organization.Url,
                
                Contact = new ContactDetails
                {                    
                    EmailAddress = organization.EmailAddress,
                    PostalAddress = organization.PostalAddress,
                    Line1 = organization.Line1,
                    Line2 = organization.Line2,
                    Street = organization.Street,
                    Postcode = organization.Postcode,
                    City = organization.City,
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
                    StoreId = store.StoreId
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

            var organizationGuid = new OrganizationId();
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
                var contactDetails = requestContent.ContactDetails;
                Dispatch(new ChangeOrganizationContactDetails(CurrentUserId, new OrganizationId(organizationId),
                    contactDetails.Line1, contactDetails.Line2, contactDetails.Street, contactDetails.Postcode, contactDetails.City,
                    contactDetails.PhoneNumber, contactDetails.EmailAddress, contactDetails.Website));
            }

            if (!String.IsNullOrWhiteSpace(requestContent.Startpage))
            {
                Dispatch(new UpdateStartpage(CurrentUserId, new OrganizationId(organizationId), requestContent.Startpage));
            }

            if (!String.IsNullOrWhiteSpace(requestContent.Plan))
            {
                Dispatch(new ChangeOrganizationPlan(CurrentUserId, new OrganizationId(organizationId),
                    requestContent.Plan));
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

        [JsonProperty("plan")]
        public string Plan { get; set; }
    }

    public class ContactDetails
    {
        [JsonProperty("postalAddress")]
        public string PostalAddress { get; set; }

        [JsonProperty("line1")]
        public string Line1 { get; set; }

        [JsonProperty("line2")]
        public string Line2 { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }
    }

    public class OrganizationsGetOkResponseContent
    {
        [JsonProperty("organizationId")]
        public Guid OrganizationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("startpage")]
        public string Startpage { get; set; }

        [JsonProperty("stores")]
        public List<Store> Stores { get; set; }

        [JsonProperty("contact")]
        public ContactDetails Contact { get; set; }
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