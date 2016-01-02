namespace Phundus.Rest.Api
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Queries;
    using Docs;
    using Organizations;

    [RoutePrefix("api/orders/{orderId}/items")]
    public class OrdersItemsController : ApiControllerBase
    {
        public IOrderQueries OrderQueries { get; set; }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(int orderId, OrdersItemPostRequestContent requestContent)
        {
            var command = new AddOrderItem
            {
                Amount = requestContent.Amount,
                ArticleId = requestContent.ArticleId,
                FromUtc = requestContent.FromUtc,
                InitiatorId = CurrentUserId,
                OrderId = orderId,
                ToUtc = requestContent.ToUtc
            };

            Dispatch(command);

            return Get(orderId, command.OrderItemId, HttpStatusCode.Created);
        }

        private HttpResponseMessage Get(int orderId, Guid orderItemId, HttpStatusCode statusCode)
        {
            var order = OrderQueries.GetById(new CurrentUserId(CurrentUserId), new OrderId(orderId));
            var item = order.Items.FirstOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format("Die Position mit der Id {0} konnte nicht gefunden werden.", orderItemId.ToString("D")));

            return Request.CreateResponse(statusCode, Map<OrderItemDoc>(item));
        }

        [PATCH("{orderItemId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(int orderId, Guid orderItemId,
            OrderItemPatchDoc doc)
        {
            Dispatcher.Dispatch(new UpdateOrderItem
            {
                Amount = doc.Amount,
                FromUtc = doc.FromUtc,
                InitiatorId = CurrentUserId,
                OrderId = orderId,
                OrderItemId = orderItemId,
                ToUtc = doc.ToUtc,
                ItemTotal = doc.ItemTotal
            });

            return Get(orderId, orderItemId, HttpStatusCode.OK);
        }

        [DELETE("{orderItemId}")]
        [Transaction]
        public virtual HttpResponseMessage Delete(int orderId, Guid orderItemId)
        {
            Dispatcher.Dispatch(new RemoveOrderItem
            {
                InitiatorId = CurrentUserId,
                OrderId = orderId,
                OrderItemId = orderItemId
            });

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}