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
    using Core.IdentityAndAccess.Organizations.Commands;
    using Core.IdentityAndAccess.Queries;
    using Core.Inventory.Queries;
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
            return new OrganizationsQueryOkResponseContent
            {
                Organizations =
                    _organizationQueries.All().Select(s => new Organization
                    {
                        Address = s.Address,
                        Name = s.Name,
                        OrganizationId = s.Guid,
                        Url = s.Url
                    }).ToList()
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
                Website = organization.Website
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
            var organizationId = Guid.NewGuid();
            Dispatch(new EstablishOrganization
            {
                InitiatorId = CurrentUserId.Id,
                OrganizationId = organizationId,
                Name = requestContent.Name
            });

            return Request.CreateResponse(HttpStatusCode.OK,
                new OrganizationsPostOkResponseContent {OrganizationId = organizationId});
        }

        [PUT("{organizationId}")]
        [Transaction]
        public virtual OrganizationDetailDto Put(Guid organizationId, [FromBody] OrganizationDetailDto value)
        {
            Dispatcher.Dispatch(new UpdateOrganizationDetails
            {
                Address = value.Address,
                DocumentTemplate = value.DocumentTemplate,
                EmailAddress = value.EmailAddress,
                InitiatorId = CurrentUserId.Id,
                OrganizationId = organizationId,
                Startpage = value.Startpage,
                Website = value.Website
            });

            return _organizationQueries.FindById(organizationId);
        }
    }

    public class OrganizationsQueryOkResponseContent
    {
        [JsonProperty("organizations")]
        public IList<Organization> Organizations { get; set; }
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
        public IList<Store> Stores { get; set; }
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