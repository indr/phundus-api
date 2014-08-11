namespace Phundus.Rest.Controllers.Shop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Queries;

    public class OrdersController : ApiControllerBase
    {
        public IOrderQueries OrderQueries { get; set; }

        public IUserQueries UserQueries { get; set; }

        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId)
        {
            var result = OrderQueries.FindByOrganizationId(organizationId, CurrentUserId);
            return Request.CreateResponse(HttpStatusCode.OK, ToDocs(result));
        }

        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId, int id)
        {
            var result = OrderQueries.FindOrder(id, organizationId, CurrentUserId);
            if (result == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order not found");

            return Request.CreateResponse(HttpStatusCode.OK, ToDoc(result));
        }

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
                ReserverId = dto.ReserverId,
                ReserverFirstName = dto.ReserverName.Split(' ').FirstOrDefault(),
                ReserverLastName = dto.ReserverName.Split(' ').LastOrDefault(),
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
                    Id = each.Id,
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
                ReserverId = each.ReserverId,
                ReserverFirstName = each.ReserverName.Split(' ').FirstOrDefault(),
                ReserverLastName = each.ReserverName.Split(' ').LastOrDefault(),
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

        public int ReserverId { get; set; }
        public string ReserverFirstName { get; set; }
        public string ReserverLastName { get; set; }
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
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public string Text { get; set; }

        public int Amount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal ItemTotal { get; set; }
    }
}