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

    [RoutePrefix("api/orders")]
    public class OrdersController : ApiControllerBase
    {
        public IOrderQueries OrderQueries { get; set; }

        public IOrderService OrderService { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get()
        {
            var result = OrderQueries.FindByUserId(CurrentUserId);
            return Request.CreateResponse(HttpStatusCode.OK, Map<ICollection<OrderDoc>>(result));
        }

        [GET("{orderId:int}")]
        [Transaction]
        public virtual HttpResponseMessage Get(int orderId)
        {
            var result = OrderQueries.FindById(orderId, CurrentUserId);
            if (result == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return Request.CreateResponse(HttpStatusCode.OK, Map<OrderDetailDoc>(result));
        }

        [GET("{orderId:int}.pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(int orderId)
        {
            var stream = OrderService.GetPdf(orderId);

            return CreatePdfResponse(stream, string.Format("Bestellung-{0}.pdf", orderId));
        }       
    }
}