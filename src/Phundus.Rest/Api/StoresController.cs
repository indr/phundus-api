namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using ContentObjects;
    using Core.Inventory.Queries;
    using Core.Inventory.Stores.Commands;
    using Newtonsoft.Json;

    [RoutePrefix("api/stores")]
    public class StoresController : ApiControllerBase
    {
        private readonly IStoreQueries _storeQueries;

        public StoresController(IStoreQueries storeQueries)
        {
            AssertionConcern.AssertArgumentNotNull(storeQueries, "StoreQueries must be provided.");

            _storeQueries = storeQueries;
        }

        [GET("")]
        [Transaction]
        public virtual StoresQueryOkResponseContent Get(Guid? ownerId)
        {
            var result = new StoresQueryOkResponseContent();
            if (!ownerId.HasValue)
                return result;

            var store = _storeQueries.FindByOwnerId(new OwnerId(ownerId.Value));
            if (store != null)
                result.Stores.Add(ToStore(store));

            return result;
        }

        private static Store ToStore(StoreDto store)
        {
            var result = new Store
            {
                Address = store.Address,
                OpeningHours = store.OpeningHours,
                StoreId = store.StoreId.Id,
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
            Dispatch(new OpenStore(new UserId(CurrentUserId), new UserId(requestContent.UserId), storeId));

            return new StoresPostOkResponseContent
            {
                StoreId = storeId.Id
            };
        }

        [PATCH("{storeId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(string storeId,
            StoresPatchRequestContent requestContent)
        {
            if (requestContent.Address != null)
            {
                Dispatch(new ChangeAddress
                {
                    InitatorId = CurrentUserId,
                    Address = requestContent.Address,
                    StoreId = new Guid(storeId)
                });
            }
            if (requestContent.Coordinate != null)
            {
                Dispatch(new ChangeCoordinate
                {
                    InitatorId = CurrentUserId,
                    Latitude = requestContent.Coordinate.Latitude,
                    Longitude = requestContent.Coordinate.Longitude,
                    StoreId = new Guid(storeId)
                });
            }
            if (requestContent.OpeningHours != null)
            {
                Dispatch(new ChangeOpeningHours
                {
                    InitatorId = CurrentUserId,
                    OpeningHours = requestContent.OpeningHours,
                    StoreId = new Guid(storeId)
                });
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
        public int UserId { get; set; }
    }

    public class StoresPostOkResponseContent
    {
        [JsonProperty("storeId")]
        public Guid StoreId { get; set; }
    }
}