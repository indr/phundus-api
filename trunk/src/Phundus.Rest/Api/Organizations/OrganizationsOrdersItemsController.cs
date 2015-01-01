namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Application.Commands;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Queries;

    [RoutePrefix("api/organizations/{organizationId}/orders/{orderId}/items/{orderItemId?}")]
    public class OrganizationsOrdersItemsController : ApiControllerBase
    {
        public IOrderQueries OrderQueries { get; set; }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(int organizationId, int orderId, OrderItemPostDoc doc)
        {
            var command = new AddOrderItem
            {
                Quantity = doc.Amount,
                ArticleId = new ArticleId(doc.ArticleId),
                FromUtc = doc.FromUtc,
                InitiatorId = new UserId(CurrentUserId),
                OrderId = new OrderId(orderId),
                ToUtc = doc.ToUtc
            };

            Dispatch(command);

            return Get(organizationId, orderId, command.ResultingOrderItemId, HttpStatusCode.Created);
        }

        private HttpResponseMessage Get(int organizationId, int orderId, Guid orderItemId, HttpStatusCode statusCode)
        {
            var order = OrderQueries.SingleByOrderIdAndOrganizationId(orderId, organizationId, CurrentUserId);
            var item = order.Items.FirstOrDefault(p => p.Id == orderItemId);
            if (item == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format("Die Position mit der Id {0} konnte nicht gefunden werden.", orderItemId.ToString("D")));

            return Request.CreateResponse(statusCode, Map<OrderItemDoc>(item));
        }

        [PATCH("")]
        [Transaction]
        public virtual HttpResponseMessage Patch(int organizationId, int orderId, Guid orderItemId,
            OrderItemPatchDoc doc)
        {
            Dispatcher.Dispatch(new UpdateOrderItem
            {
                Quantity = doc.Amount,
                FromUtc = doc.FromUtc,
                InitiatorId = new UserId(CurrentUserId),
                OrderId = orderId,
                OrderItemId = orderItemId,
                ToUtc = doc.ToUtc
            });

            return Get(organizationId, orderId, orderItemId, HttpStatusCode.OK);
        }

        [DELETE("")]
        [Transaction]
        public virtual HttpResponseMessage Delete(int organizationId, int orderId, Guid orderItemId)
        {
            Dispatcher.Dispatch(new RemoveOrderItem
            {
                InitiatorId = new UserId(CurrentUserId),
                OrderId = orderId,
                OrderItemId = orderItemId
            });

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}