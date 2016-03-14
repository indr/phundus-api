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
    using MsgReader.Outlook;
    using Newtonsoft.Json;

    [RoutePrefix("api/stores")]
    public class StoresController : ApiControllerBase
    {
        private readonly IStoresQueries _storesQueries;

        public StoresController(IStoresQueries storesQueries)
        {
            _storesQueries = storesQueries;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<Store> Get(Guid? ownerId)
        {
            var result = new QueryOkResponseContent<Store>();
            if (!ownerId.HasValue)
                return result;

            var store = _storesQueries.FindByOwnerId(ownerId.Value);
            if (store != null)
                result.Results.Add(ToStore(store));

            return result;
        }

        private static Store ToStore(StoreData store)
        {
            var result = new Store
            {
                Name = store.Name,
                Address = store.Address,
                OpeningHours = store.OpeningHours,
                StoreId = store.StoreId,
            };
            if (store.Latitude.HasValue && store.Longitude.HasValue)
            {
                result.Coordinate = new Coordinate
                {
                    Latitude = store.Latitude.Value,
                    Longitude = store.Longitude.Value
                };
            }
            return result;
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
            if (rq.Address != null)
            {
                Dispatch(new ChangeAddress(CurrentUserId, storeId, rq.Address));
            }
            if (rq.ContactDetails != null)
            {
                var cd = rq.ContactDetails;
                Dispatch(new ChangeContactDetails(CurrentUserId, storeId, cd.EmailAddress, cd.PhoneNumber, cd.Line1, cd.Line2, cd.Street, cd.Postcode, cd.City));
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

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("contactDetails")]
        public ContactDetails ContactDetails { get; set; }

        [JsonProperty("coordinate")]
        public Coordinate Coordinate { get; set; }

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