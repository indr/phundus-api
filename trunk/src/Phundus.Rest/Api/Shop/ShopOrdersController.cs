namespace Phundus.Rest.Api.Shop
{
    using System;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Domain.Model;
    using Newtonsoft.Json;
    using Phundus.Shop.Orders.Commands;

    [RoutePrefix("api/shop/orders")]
    public class ShopOrdersController : ApiControllerBase
    {
        [POST("")]
        [Transaction]
        public virtual ShopOrdersPostOkResponseContent Post(ShopOrdersPostRequestContent requestContent)
        {
            var command = new PlaceOrder(CurrentUserId, new LessorId(requestContent.LessorId));

            Dispatch(command);

            return new ShopOrdersPostOkResponseContent
            {
                OrderId = command.ResultingOrderId
            };
        }

        public class ShopOrdersPostOkResponseContent
        {
            [JsonProperty("orderId")]
            public int OrderId { get; set; }
        }

        public class ShopOrdersPostRequestContent
        {
            [JsonProperty("lessorId")]
            public Guid LessorId { get; set; }
        }
    }
}