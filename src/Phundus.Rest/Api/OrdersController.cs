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
    using Common.Domain.Model;
    using Common.Resources;
    using ContentObjects;
    using IdentityAccess.Application;
    using Newtonsoft.Json;
    using Phundus.Shop.Application;

    [RoutePrefix("api/orders")]
    public class OrdersController : ApiControllerBase
    {
        private readonly IMembershipQueryService _membershipQueryService;
        private readonly IOrderPdfService _orderPdfService;
        private readonly IOrderQueryService _orderQueryService;
        private readonly IShortIdGeneratorService _shortIdGeneratorService;

        public OrdersController(IOrderQueryService orderQueryService, IOrderPdfService orderPdfService,
            IShortIdGeneratorService shortIdGeneratorService, IMembershipQueryService membershipQueryService)
        {
            _orderQueryService = orderQueryService;
            _orderPdfService = orderPdfService;
            _shortIdGeneratorService = shortIdGeneratorService;
            _membershipQueryService = membershipQueryService;
        }

        [GET("")]
        [Transaction]
        public virtual QueryOkResponseContent<Order> Get()
        {
            UserId queryUserId = null;
            OrganizationId queryOrganizationId = null;

            var queryParams = GetQueryParams();
            if (queryParams.ContainsKey("userId"))
                queryUserId = new UserId(Guid.Parse(queryParams["userId"]));
            if (queryParams.ContainsKey("organizationId"))
                queryOrganizationId = new OrganizationId(Guid.Parse(queryParams["organizationId"]));

            var orders = _orderQueryService.Query(CurrentUserId, null, queryUserId, queryOrganizationId).ToList();
            return new QueryOkResponseContent<Order>(Map<IList<Order>>(orders));
        }

        [GET("{orderId}")]
        [Transaction]
        public virtual HttpResponseMessage Get(Guid orderId)
        {
            var order = _orderQueryService.GetById(CurrentUserId, new OrderId(orderId));
            var membership = _membershipQueryService.Find(order.LessorId, order.LesseeId);

            var result = Map<OrderDetail>(order);
            result.IsMember = membership != null;
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [GET("{orderId}/pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(Guid orderId)
        {
            var orderPdf = _orderPdfService.GetOrderPdf(CurrentUserId, new OrderId(orderId));

            return CreatePdfResponse(orderPdf.Stream, string.Format("Bestellung-{0}.pdf", orderPdf.OrderShortId));
        }

        [POST("")]
        public virtual PostOkResponseContent Post(PostRequestContent requestContent)
        {
            var orderId = new OrderId();
            var orderShortId = _shortIdGeneratorService.GetNext<OrderShortId>();

            var command = new CreateEmptyOrder(CurrentUserId,
                orderId, orderShortId,
                new LessorId(requestContent.LessorId),
                new LesseeId(requestContent.LesseeId));

            Dispatch(command);

            return new PostOkResponseContent
            {
                OrderId = orderId.Id,
                OrderShortId = orderShortId.Id
            };
        }

        [PATCH("{orderId}")]
        public virtual HttpResponseMessage Patch(Guid orderId, PatchRequestContent requestContent)
        {
            if (requestContent.Status == "Rejected")
                Dispatch(new RejectOrder(CurrentUserId, new OrderId(orderId)));
            else if (requestContent.Status == "Approved")
                Dispatch(new ApproveOrder(CurrentUserId, new OrderId(orderId)));
            else if (requestContent.Status == "Closed")
                Dispatch(new CloseOrder(CurrentUserId, new OrderId(orderId)));
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Unbekannter Status \"" + requestContent.Status + "\"");

            return Get(orderId);
        }

        public class PatchRequestContent
        {
            [JsonProperty("status")]
            public string Status { get; set; }
        }

        public class PostOkResponseContent
        {
            [JsonProperty("orderId")]
            public Guid OrderId { get; set; }

            [JsonProperty("orderShortId")]
            public int OrderShortId { get; set; }
        }

        public class PostRequestContent
        {
            [JsonProperty("lessorId")]
            public Guid LessorId { get; set; }

            [JsonProperty("lesseeId")]
            public Guid LesseeId { get; set; }
        }
    }
}