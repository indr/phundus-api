namespace Phundus.Rest.Controllers.Shop
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Queries;

    [RoutePrefix("api/organizations/{organizationId}/orders/{orderId}/items/{orderItemId?}")]
    public class OrdersItemsController : ApiControllerBase
    {
        public IOrderQueries OrderQueries { get; set; }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(int organizationId, int orderId, OrderItemPostDoc doc)
        {
            var command = new AddOrderItem
            {
                Amount = doc.Amount,
                ArticleId = doc.ArticleId,
                From = doc.From,
                InitiatorId = CurrentUserId,
                OrderId = orderId,
                To = doc.To
            };

            Dispatch(command);

            return Get(organizationId, orderId, command.OrderItemId, HttpStatusCode.Created);

            
        }

        private HttpResponseMessage Get(int organizationId, int orderId, Guid orderItemId, HttpStatusCode statusCode)
        {
            var order = OrderQueries.FindById(orderId, CurrentUserId);
            var item = order.Items.FirstOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format("Die Position mit der Id {0} konnte nicht gefunden werden.", orderItemId.ToString("D")));

            return Request.CreateResponse(statusCode, new OrderItemDoc
            {
                Amount = item.Amount,
                ArticleId = item.ArticleId,
                From = item.From,
                OrderItemId = item.Id,
                Text = item.Text,
                To = item.To,
                ItemTotal = item.LineTotal,
                OrderId = item.OrderId,
                UnitPrice = item.UnitPrice
            });
        }

        [PATCH("")]
        [Transaction]
        public virtual HttpResponseMessage Patch(int organizationId, int orderId, Guid orderItemId,
            OrderItemPatchDoc doc)
        {
            Dispatcher.Dispatch(new UpdateOrderItem
            {
                Amount = doc.Amount,
                From = doc.From,
                InitiatorId = CurrentUserId,
                OrderId = orderId,
                OrderItemId = orderItemId,
                To = doc.To
            });

            return Get(organizationId, orderId, orderItemId, HttpStatusCode.OK);
        }

        [DELETE("")]
        [Transaction]
        public virtual HttpResponseMessage Delete(int organizationId, int orderId, Guid orderItemId)
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

    public class OrderItemPostDoc
    {
        public int ArticleId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Amount { get; set; }
    }

    public class OrderItemPatchDoc
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Amount { get; set; }
    }
}