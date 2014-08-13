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
            return Request.CreateResponse(HttpStatusCode.OK, ToDocs(result));
        }

        [GET("{orderId:int}")]
        [Transaction]
        public virtual HttpResponseMessage Get(int orderId)
        {
            var result = OrderQueries.FindById(orderId, CurrentUserId);
            if (result == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return Request.CreateResponse(HttpStatusCode.OK, ToDoc(result));
        }

        [GET("{orderId:int}.pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(int organizationId, int orderId)
        {
            var stream = OrderService.GetPdf(orderId);

            return CreatePdfResponse(stream, string.Format("Bestellung-{0}", orderId));
        }

        private object ToDoc(OrderDto dto)
        {
            var result = new OrderDetailDoc
            {
                OrderId = dto.Id,
                Version = dto.Version,
                OrganizationId = dto.OrganizationId,
                CreatedOn = dto.CreateDate,
                ModifiedOn = dto.ModifiedOn,
                Status = dto.Status.ToString(),
                TotalPrice = dto.TotalPrice
            };
            foreach (var each in dto.Items)
            {
                result.Items.Add(new OrderItemDoc
                {
                    Amount = each.Amount,
                    ArticleId = each.ArticleId,
                    From = each.From,
                    OrderItemId = each.Id,
                    Text = each.Text,
                    To = each.To,
                    ItemTotal = each.LineTotal,
                    UnitPrice = each.UnitPrice
                });
            }
            return result;
        }

        private static ICollection<OrderDoc> ToDocs(IEnumerable<OrderDto> dtos)
        {
            return dtos.Select(each => new OrderDoc
            {
                OrderId = each.Id,
                Version = each.Version,
                OrganizationId = each.OrganizationId,
                CreatedOn = each.CreateDate,
                ModifiedOn = each.ModifiedOn,
                Status = each.Status.ToString()
            }).ToList();
        }

        public class OrderDetailDoc : OrderDoc
        {
            private IList<OrderItemDoc> _items = new List<OrderItemDoc>();

            public IList<OrderItemDoc> Items
            {
                get { return _items; }
                set { _items = value; }
            }

            public decimal TotalPrice { get; set; }
        }

        public class OrderDoc
        {
            public int OrderId { get; set; }
            public int Version { get; set; }
            public int OrganizationId { get; set; }

            public string Status { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime? ModifiedOn { get; set; }
        }

        public class OrderItemDoc
        {
            public int OrderId { get; set; }
            public Guid OrderItemId { get; set; }

            public int ArticleId { get; set; }
            public string Text { get; set; }

            public int Amount { get; set; }
            public DateTime From { get; set; }
            public DateTime To { get; set; }

            public decimal UnitPrice { get; set; }
            public decimal ItemTotal { get; set; }
        }
    }
}