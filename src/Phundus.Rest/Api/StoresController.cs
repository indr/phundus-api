namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using ContentObjects;
    using Inventory.Projections;
    using Inventory.Stores.Commands;
    using Newtonsoft.Json;

    [RoutePrefix("api/stores")]
    public class StoresController : ApiControllerBase
    {
        private readonly IStoresQueries _storesQueries;

        public StoresController(IStoresQueries storesQueries)
        {
            if (storesQueries == null) throw new ArgumentNullException("storesQueries");
            _storesQueries = storesQueries;
        }

        [GET("")]
        [Transaction]
        public virtual StoresQueryOkResponseContent Get(Guid? ownerId)
        {
            var result = new StoresQueryOkResponseContent();
            if (!ownerId.HasValue)
                return result;

            var store = _storesQueries.FindByOwnerId(new OwnerId(ownerId.Value));
            if (store != null)
                result.Stores.Add(ToStore(store));

            return result;
        }

        private static Store ToStore(StoreData store)
        {
            var result = new Store
            {
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
        [Transaction]
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
        [Transaction]
        public virtual HttpResponseMessage Patch(Guid storeId, StoresPatchRequestContent requestContent)
        {
            if (requestContent.Address != null)
            {
                Dispatch(new ChangeAddress(CurrentUserId, new StoreId(storeId), requestContent.Address));
            }
            if (requestContent.Coordinate != null)
            {
                Dispatch(new ChangeCoordinate(CurrentUserId, new StoreId(storeId), requestContent.Coordinate.Latitude, requestContent.Coordinate.Longitude));
            }
            if (requestContent.OpeningHours != null)
            {
                Dispatch(new ChangeOpeningHours(CurrentUserId, new StoreId(storeId), requestContent.OpeningHours));
            }

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }

    public class StoresQueryOkResponseContent
    {
        public StoresQueryOkResponseContent()
        {
            Stores = new List<Store>();
        }

        [JsonProperty("stores")]
        public IList<Store> Stores { get; set; }
    }

    public class StoresPatchRequestContent
    {
        [JsonProperty("address")]
        public string Address { get; set; }

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