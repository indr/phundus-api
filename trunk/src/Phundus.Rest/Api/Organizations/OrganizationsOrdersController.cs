namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Orders;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Queries;

    [RoutePrefix("api/organizations/{organizationId}/orders")]
    public class OrganizationsOrdersController : ApiControllerBase
    {
        public IOrderQueries OrderQueries { get; set; }

        public IUserQueries UserQueries { get; set; }

        public IOrderService OrderService { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId)
        {
            var result = OrderQueries.FindByOrganizationId(organizationId, CurrentUserId);
            return Request.CreateResponse(HttpStatusCode.OK, ToDocs(result));
        }

        [GET("{orderId:int}")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId, int orderId)
        {
            var result = OrderQueries.FindOrder(orderId, organizationId, CurrentUserId);
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order not found");

            return Request.CreateResponse(HttpStatusCode.OK, ToDoc(result));
        }

        [GET("{orderId:int}.pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(int organizationId, int orderId)
        {
            var stream = OrderService.GetPdf(organizationId, orderId, CurrentUserId);

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = string.Format("Bestelung-{0}.pdf", orderId)
            };

            return result;
        }

        [PATCH("{orderId}")]
        [Transaction]
        public virtual HttpResponseMessage Patch(int organizationId, int orderId, OrderPatchDoc doc)
        {
            if (doc.Status == "Rejected")
                Dispatch(new RejectOrder {InitiatorId = CurrentUserId, OrderId = orderId});
            else if (doc.Status == "Approved")
                Dispatch(new ApproveOrder {InitiatorId = CurrentUserId, OrderId = orderId});
            else if (doc.Status == "Closed")
                Dispatch(new CloseOrder {InitiatorId = CurrentUserId, OrderId = orderId});
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    "Unbekannter Status \"" + doc.Status + "\"");

            return Get(organizationId, orderId);
        }

        public class OrderPatchDoc
        {
            public string Status { get; set; }
        }

        [POST("")]
        [Transaction]
        public virtual HttpResponseMessage Post(int organizationId, OrdersPostDoc doc)
        {
            int userId;
            if (!Int32.TryParse(doc.UserName, out userId))
            {
                var user = UserQueries.ByUserName(doc.UserName);
                if (user == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                        string.Format("Der Benutzer mit der E-Mail-Adresse \"{0}\" konnte nicht gefunden werden.",
                            doc.UserName));

                userId = user.Id;
            }

            var command = new CreateEmptyOrder
            {
                InitiatorId = CurrentUserId,
                OrganizationId = organizationId,
                UserId = userId
            };

            Dispatcher.Dispatch(command);

            return Get(organizationId, command.OrderId);
        }

        private static OrderDetailDoc ToDoc(OrderDto dto)
        {
            var result = new OrderDetailDoc
            {
                OrderId = dto.Id,
                Version = dto.Version,
                OrganizationId = dto.OrganizationId,
                CreatedOn = dto.CreateDate,
                BorrowerId = dto.Borrower.BorrowerId,
                BorrowerFirstName = dto.Borrower.FirstName,
                BorrowerLastName = dto.Borrower.LastName,
                BorrowerMemberNumber = dto.Borrower.MemberNumber,
                Status = dto.Status.ToString(),
                TotalPrice = dto.TotalPrice
            };
            foreach (var each in dto.Items)
            {
                result.Items.Add(new OrderItemDoc
                {
                    Amount = each.Amount,
                    ArticleId = each.ArticleId,
                    Availability = each.Availability,
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
                BorrowerId = each.Borrower.BorrowerId,
                BorrowerFirstName = each.Borrower.FirstName,
                BorrowerLastName = each.Borrower.LastName,
                BorrowerMemberNumber = each.Borrower.MemberNumber,
                Status = each.Status.ToString()
            }).ToList();
        }
    }

    

    public class OrdersPostDoc
    {
        public string UserName { get; set; }
    }

    public class OrderDoc
    {
        public int OrderId { get; set; }
        public int Version { get; set; }
        public int OrganizationId { get; set; }

        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }

        public int BorrowerId { get; set; }
        public string BorrowerFirstName { get; set; }
        public string BorrowerLastName { get; set; }
        public string BorrowerMemberNumber { get; set; }
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
        public bool Availability { get; set; }
    }
}