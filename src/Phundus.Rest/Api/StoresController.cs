namespace Phundus.Rest.Api
{
    using System;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using ContentObjects;
    using Inventory.Application;
    using Inventory.Projections;
    using Newtonsoft.Json;

    [RoutePrefix("api/stores")]
    public class StoresController : ApiControllerBase
    {
        private readonly IStoresQueryService _storesQueryService;

        public StoresController(IStoresQueryService storesQueryService)
        {
            _storesQueryService = storesQueryService;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<StoreDetailsCto> Get(Guid? ownerId)
        {
            var stores = _storesQueryService.Query(ownerId);
            return new QueryOkResponseContent<StoreDetailsCto>(Map<StoreDetailsCto[]>(stores));
        }

        [POST("")]
        public virtual StoresPostOkResponseContent Post(StoresPostRequestContent requestContent)
        {
            var storeId = new StoreId();
            Dispatch(new OpenStore(CurrentUserId, new OwnerId(requestContent.UserId), storeId));

            return new StoresPostOkResponseContent
            {
                StoreId = storeId.Id
            };
        }

        [PATCH("{storeId}")]
        public virtual HttpResponseMessage Patch(StoreId storeId, StoresPatchRequestContent rq)
        {
            if (rq.Name != null)
            {
                Dispatch(new RenameStore(CurrentUserId, storeId, rq.Name));
            }
            if (rq.Contact != null)
            {
                var cd = rq.Contact;
                Dispatch(new ChangeContactDetails(CurrentUserId, storeId, cd.EmailAddress, cd.PhoneNumber, cd.Line1,
                    cd.Line2, cd.Street, cd.Postcode, cd.City));
            }
            if (rq.Coordinate != null)
            {
                Dispatch(new ChangeCoordinate(CurrentUserId, storeId, rq.Coordinate.Latitude,
                    rq.Coordinate.Longitude));
            }
            if (rq.OpeningHours != null)
            {
                Dispatch(new ChangeOpeningHours(CurrentUserId, storeId, rq.OpeningHours));
            }

            return NoContent();
        }
    }

    public class StoresPatchRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contact")]
        public ContactCto Contact { get; set; }

        [JsonProperty("coordinate")]
        public CoordinateCto Coordinate { get; set; }

        [JsonProperty("openingHours")]
        public string OpeningHours { get; set; }
    }

    public class StoresPostRequestContent
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
    }

    public class StoresPostOkResponseContent
    {
        [JsonProperty("storeId")]
        public Guid StoreId { get; set; }
    }
}