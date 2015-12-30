namespace Phundus.Rest.Api
{
    using System;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Inventory.Stores.Commands;
    using Newtonsoft.Json;

    [RoutePrefix("api/stores")]
    public class StoresController : ApiControllerBase
    {
        [POST("")]
        [Transaction]
        public virtual StoresPostOkResponseContent Post(StoresPostRequestContent requestContent)
        {
            var storeId = Guid.NewGuid().ToString("N");
            Dispatch(new OpenStore
            {
                InitiatorId = CurrentUserId,
                UserId = Convert.ToInt32(requestContent.UserId),
                StoreId = storeId
            });

            return new StoresPostOkResponseContent
            {
                StoreId = storeId
            };
        }

        [PUT("{storeId}/address")]
        [Transaction]
        public virtual HttpResponseMessage PutAddress(string storeId,
            StoresAddressPutRequestContent requestContent)
        {
            Dispatch(new ChangeAddress
            {
                InitatorId = CurrentUserId,
                Address = requestContent.Address,
                StoreId = new Guid(storeId)
            });

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [PUT("{storeId}/coordinate")]
        [Transaction]
        public virtual HttpResponseMessage PutCoordinate(string storeId,
            StoresCoordinatePutRequestContent requestContent)
        {
            Dispatch(new ChangeCoordinate
            {
                InitatorId = CurrentUserId,
                Latitude = requestContent.Coordinate.Latitude,
                Longitude = requestContent.Coordinate.Longitude,
                StoreId = new Guid(storeId)
            });

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [PUT("{storeId}/opening-hours")]
        [Transaction]
        public virtual HttpResponseMessage PutOpeningHours(string storeId,
            StoresOpeningHoursPutRequestContent requestContent)
        {
            Dispatch(new ChangeOpeningHours
            {
                InitatorId = CurrentUserId,
                OpeningHours = requestContent.OpeningHours,
                StoreId = new Guid(storeId)
            });

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }

    public class StoresPostRequestContent
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }

    public class StoresPostOkResponseContent
    {
        [JsonProperty("storeId")]
        public string StoreId { get; set; }
    }

    public class StoresGetOkResponseContent
    {
        [JsonProperty("storeId")]
        public string StoreId { get; set; }
    }

    public class StoresAddressPutRequestContent
    {
        [JsonProperty("address")]
        public string Address { get; set; }
    }

    public class StoresCoordinatePutRequestContent
    {
        [JsonProperty("coordinate")]
        public Coordinate Coordinate { get; set; }
    }

    public class StoresOpeningHoursPutRequestContent
    {
        [JsonProperty("openingHours")]
        public string OpeningHours { get; set; }
    }
}