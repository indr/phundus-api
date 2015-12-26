namespace Phundus.Rest.Api
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Inventory.Stores.Commands;
    using Newtonsoft.Json;

    [RoutePrefix("api/stores")]
    public class StoresController : ApiControllerBase
    {
        [GET("{storeId}")]
        [Transaction]
        public virtual StoresGetOkResponseContent Get(string storeId)
        {
            throw new HttpException((int) HttpStatusCode.NotImplemented, "Not implemented.");
        }

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
        public virtual HttpResponse PutAddress(string storeId)
        {
            throw new HttpException((int)HttpStatusCode.NotImplemented, "Not implemented.");
        }

        [PUT("{storeId}/opening-hours")]
        [Transaction]
        public virtual HttpResponse PutOpeningHours(string storeId)
        {
            throw new HttpException((int)HttpStatusCode.NotImplemented, "Not implemented.");
        }

        [PUT("{storeId}/coordinate")]
        [Transaction]
        public virtual StoresCoordinatePutOkResponseContent PutCoordinate(string storeId, StoresCoordinatePutRequestContent requestContent)
        {
            Dispatch(new ChangeCoordinate
            {
                InitatorId = CurrentUserId,
                Latitude = requestContent.Coordinate.Latitude,
                Longitude = requestContent.Coordinate.Longitude,
                StoreId = new Guid(storeId)
            });

            return new StoresCoordinatePutOkResponseContent();
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

    public class StoresCoordinatePutRequestContent
    {
        [JsonProperty("coordinate")]
        public Coordinate Coordinate { get; set; }
    }

    public class StoresCoordinatePutOkResponseContent
    {
        
    }
}