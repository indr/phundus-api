namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using ContentObjects;
    using IdentityAccess.Application;
    using IdentityAccess.Projections;
    using Inventory.Application;
    using Inventory.Model;
    using Inventory.Model.Collaborators;
    using Inventory.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/organizations")]
    public class OrganizationsController : ApiControllerBase
    {
        private readonly IOrganizationQueries _organizationQueries;
        private readonly IStoresQueries _storesQueries;
        private readonly IOwnerService _ownerService;

        public OrganizationsController(IOrganizationQueries organizationQueries, IStoresQueries storesQueries, IOwnerService ownerService)
        {
            _organizationQueries = organizationQueries;
            _storesQueries = storesQueries;
            _ownerService = ownerService;
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
        [Authorize(Roles = "Admin")]
        public virtual HttpResponseMessage Post(OrganizationsPostRequestContent rq)
        {
            var organizationId = new OrganizationId();
            Dispatch(new EstablishOrganization(CurrentUserId, organizationId, rq.Name));

            // Ugly ugly ugly... The drawback of eventual consistency...
            // Inventory context should listen to OrganizationEstablished events and create
            // the corresponding store...
            var ownerId = new OwnerId(organizationId.Id);
            for (var i = 1; i <= 10; i++)
            {
                if (_ownerService.FindById(ownerId) != null)
                    break;

                Thread.Sleep(i*200);
            }

            var storeId = new StoreId();
            Dispatch(new OpenStore(CurrentUserId, ownerId, storeId));

            return Created(new OrganizationsPostOkResponseContent {OrganizationId = organizationId.Id});
        }

        [PATCH("{organizationId}")]
        public virtual HttpResponseMessage Patch(Guid organizationId, OrganizationsPatchRequestContent rq)
        {
            if (rq.ContactDetails != null)
            {
                var cd = rq.ContactDetails;
                Dispatch(new ChangeOrganizationContactDetails(CurrentUserId, new OrganizationId(organizationId),
                    cd.Line1, cd.Line2, cd.Street, cd.Postcode, cd.City, cd.PhoneNumber, cd.EmailAddress, cd.Website));
            }

            if (!String.IsNullOrWhiteSpace(rq.Startpage))
            {
                Dispatch(new UpdateStartpage(CurrentUserId, new OrganizationId(organizationId), rq.Startpage));
            }

            if (!String.IsNullOrWhiteSpace(rq.Plan))
            {
                Dispatch(new ChangeOrganizationPlan(CurrentUserId, new OrganizationId(organizationId), rq.Plan));
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