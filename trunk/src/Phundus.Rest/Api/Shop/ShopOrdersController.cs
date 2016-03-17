namespace Phundus.Rest.Api.Shop
{
    using System;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common.Domain.Model;
    using Common.Resources;
    using Newtonsoft.Json;
    using Phundus.Shop.Application;

    [RoutePrefix("api/shop/orders")]
    public class ShopOrdersController : ApiControllerBase
    {
        private readonly IShortIdGeneratorService _shortIdGeneratorService;

        public ShopOrdersController(IShortIdGeneratorService shortIdGeneratorService)
        {            
            _shortIdGeneratorService = shortIdGeneratorService;
        }

        [POST("")]
        public virtual ShopOrdersPostOkResponseContent Post(ShopOrdersPostRequestContent requestContent)
        {
            var orderId = new OrderId();
            var orderShortId = _shortIdGeneratorService.GetNext<OrderShortId>();

            Dispatch(new PlaceOrder(CurrentUserId, orderId, orderShortId, new LessorId(requestContent.LessorId)));

            return new ShopOrdersPostOkResponseContent
            {
                OrderId = orderId.Id,
                OrderShortId = orderShortId.Id
            };
        }

        public class ShopOrdersPostOkResponseContent
        {
            [JsonProperty("orderId")]
            public Guid OrderId { get; set; }

            [JsonProperty("orderShortId")]
            public int OrderShortId { get; set; }
        }

        public class ShopOrdersPostRequestContent
        {
            [JsonProperty("lessorId")]
            public Guid LessorId { get; set; }
        }
    }
}