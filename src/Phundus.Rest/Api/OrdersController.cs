namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Shop.Orders;
    using Core.Shop.Queries;
    using Docs;
    using Newtonsoft.Json;

    [RoutePrefix("api/orders")]
    public class OrdersController : ApiControllerBase
    {
        public IOrderQueries OrderQueries { get; set; }

        public IPdfStore PdfStore { get; set; }

        [GET("")]
        [Transaction]
        public virtual OrdersQueryOkResponseContent Get()
        {
            var userId = (int?) null;
            var organizationId = (Guid?) null;

            var queryParams = Request.GetQueryNameValuePairs().ToDictionary(ks => ks.Key, es => es.Value);
            if (queryParams.ContainsKey("userId"))
                userId = Convert.ToInt32(queryParams["userId"]);
            if (queryParams.ContainsKey("organizationId"))
                organizationId = Guid.Parse(queryParams["organizationId"]);

            var orders = OrderQueries.Query(CurrentUserId, null, userId, organizationId).ToList();
            var result = new OrdersQueryOkResponseContent
            {
                Orders = Map<IList<OrderDoc>>(orders)
            };
            return result;
        }

        [GET("{orderId:int}")]
        [Transaction]
        public virtual HttpResponseMessage Get(int orderId)
        {
            var result = OrderQueries.SingleByOrderId(orderId, CurrentUserId);
            if (result == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return Request.CreateResponse(HttpStatusCode.OK, Map<OrderDetailDoc>(result));
        }

        [GET("{orderId:int}.pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(int orderId)
        {
            var result = PdfStore.GetOrderPdf(orderId, CurrentUserId);
            if (result == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return CreatePdfResponse(result, string.Format("Bestellung-{0}.pdf", orderId));
        }
    }

    public class OrdersQueryOkResponseContent
    {
        [JsonProperty("orders")]
        public IList<OrderDoc> Orders { get; set; } 
    }

    public class Order
    {
        [JsonProperty("orderId")]
        public int OrderId { get; set; }

        [JsonProperty("createdAtUtc")]
        public DateTime CreatedAtUtc { get; set; }

        [JsonProperty("lessorName")]
        public string LessorName { get; set; }

        [JsonProperty("lesseeName")]
        public string LesseeName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}