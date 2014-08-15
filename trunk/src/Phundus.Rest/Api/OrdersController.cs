namespace Phundus.Rest.Api
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Shop.Orders;
    using Core.Shop.Queries;
    using Docs;

    [RoutePrefix("api/orders")]
    public class OrdersController : ApiControllerBase
    {
        public IOrderQueries OrderQueries { get; set; }

        public IPdfStore PdfStore { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get()
        {
            var result = OrderQueries.ManyByUserId(CurrentUserId);
            return Request.CreateResponse(HttpStatusCode.OK, Map<ICollection<OrderDoc>>(result));
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
            // TODO: Read-Access-Security
            var stream = PdfStore.GetOrderPdf(orderId);

            return CreatePdfResponse(stream, string.Format("Bestellung-{0}.pdf", orderId));
        }
    }
}