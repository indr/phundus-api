namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using AutoMapper;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;
    using Core.Shop.Orders;
    using Core.Shop.Orders.Commands;
    using Core.Shop.Queries;

    [RoutePrefix("api/organizations/{organizationId}/orders")]
    public class OrganizationsOrdersController : ApiControllerBase
    {
        static OrganizationsOrdersController()
        {
            Mapper.CreateMap<OrderDto, OrderDoc>()
                .ForMember(d => d.BorrowerFirstName, o => o.MapFrom(s => s.Borrower.FirstName))
                .ForMember(d => d.BorrowerId, o => o.MapFrom(s => s.Borrower.BorrowerId))
                .ForMember(d => d.BorrowerLastName, o => o.MapFrom(s => s.Borrower.LastName))
                .ForMember(d => d.BorrowerMemberNumber, o => o.MapFrom(s => s.Borrower.MemberNumber))
                .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreateDate))
                .ForMember(d => d.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.OrganizationId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Version, o => o.MapFrom(s => s.Version));

            Mapper.CreateMap<OrderItemDto, OrderItemDoc>()
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
                .ForMember(d => d.ArticleId, o => o.MapFrom(s => s.ArticleId))
                .ForMember(d => d.Availability, o => o.MapFrom(s => s.Availability))
                .ForMember(d => d.From, o => o.MapFrom(s => s.From))
                .ForMember(d => d.ItemTotal, o => o.MapFrom(s => s.LineTotal))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.OrderItemId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Text))
                .ForMember(d => d.To, o => o.MapFrom(s => s.To))
                .ForMember(d => d.UnitPrice, o => o.MapFrom(s => s.UnitPrice));

            Mapper.CreateMap<OrderDto, OrderDetailDoc>()
                .ForMember(d => d.BorrowerFirstName, o => o.MapFrom(s => s.Borrower.FirstName))
                .ForMember(d => d.BorrowerId, o => o.MapFrom(s => s.Borrower.BorrowerId))
                .ForMember(d => d.BorrowerLastName, o => o.MapFrom(s => s.Borrower.LastName))
                .ForMember(d => d.BorrowerMemberNumber, o => o.MapFrom(s => s.Borrower.MemberNumber))
                .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.CreateDate))
                .ForMember(d => d.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.OrganizationId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Version, o => o.MapFrom(s => s.Version))
                .ForMember(d => d.TotalPrice, o => o.MapFrom(s => s.TotalPrice))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));
        }

        public IOrderQueries OrderQueries { get; set; }

        public IUserQueries UserQueries { get; set; }

        public IOrderService OrderService { get; set; }

        [GET("")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId)
        {
            var result = OrderQueries.FindByOrganizationId(organizationId, CurrentUserId);
            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<ICollection<OrderDoc>>(result));
        }

        [GET("{orderId:int}")]
        [Transaction]
        public virtual HttpResponseMessage Get(int organizationId, int orderId)
        {
            var result = OrderQueries.FindOrder(orderId, organizationId, CurrentUserId);
            if (result == null)
                return CreateNotFoundResponse("Die Bestellung mit der Id {0} konnte nicht gefunden werden.", orderId);

            return Request.CreateResponse(HttpStatusCode.OK, Mapper.Map<OrderDetailDoc>(result));
        }

        [GET("{orderId:int}.pdf")]
        [Transaction]
        public virtual HttpResponseMessage GetPdf(int organizationId, int orderId)
        {
            var stream = OrderService.GetPdf(organizationId, orderId, CurrentUserId);

            return CreatePdfResponse(stream, string.Format("Bestellung-{0}", orderId));
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

        //private static OrderDetailDoc ToDoc(OrderDto dto)
        //{
        //    var result = new OrderDetailDoc
        //    {
        //        OrderId = dto.Id,
        //        Version = dto.Version,
        //        OrganizationId = dto.OrganizationId,
        //        CreatedOn = dto.CreateDate,
        //        ModifiedOn = dto.ModifiedOn,
        //        BorrowerId = dto.Borrower.BorrowerId,
        //        BorrowerFirstName = dto.Borrower.FirstName,
        //        BorrowerLastName = dto.Borrower.LastName,
        //        BorrowerMemberNumber = dto.Borrower.MemberNumber,
        //        Status = dto.Status.ToString(),
        //        TotalPrice = dto.TotalPrice
        //    };
        //    foreach (var each in dto.Items)
        //    {
        //        result.Items.Add(new OrderItemDoc
        //        {
        //            Amount = each.Amount,
        //            ArticleId = each.ArticleId,
        //            Availability = each.Availability,
        //            From = each.From,
        //            OrderItemId = each.Id,
        //            Text = each.Text,
        //            To = each.To,
        //            ItemTotal = each.LineTotal,
        //            UnitPrice = each.UnitPrice
        //        });
        //    }
        //    return result;
        //}

        //private static ICollection<OrderDoc> ToDocs(IEnumerable<OrderDto> dtos)
        //{
        //    return dtos.Select(each => new OrderDoc
        //    {
        //        OrderId = each.Id,
        //        Version = each.Version,
        //        OrganizationId = each.OrganizationId,
        //        CreatedOn = each.CreateDate,
        //        ModifiedOn = each.ModifiedOn,
        //        BorrowerId = each.Borrower.BorrowerId,
        //        BorrowerFirstName = each.Borrower.FirstName,
        //        BorrowerLastName = each.Borrower.LastName,
        //        BorrowerMemberNumber = each.Borrower.MemberNumber,
        //        Status = each.Status.ToString()
        //    }).ToList();
        //}

        public class OrderDoc
        {
            public int OrderId { get; set; }
            public int Version { get; set; }
            public int OrganizationId { get; set; }

            public string Status { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime? ModifiedOn { get; set; }

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

        public class OrderPatchDoc
        {
            public string Status { get; set; }
        }

        public class OrdersPostDoc
        {
            public string UserName { get; set; }
        }
    }
}