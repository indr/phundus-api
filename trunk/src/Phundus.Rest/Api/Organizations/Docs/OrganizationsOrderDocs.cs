namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core.Shop.Queries;

    public class OrganizationOrderDocsProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OrderDto, OrderDoc>()
                .ForMember(d => d.BorrowerFirstName, o => o.MapFrom(s => s.Borrower_FirstName))
                .ForMember(d => d.BorrowerId, o => o.MapFrom(s => s.Borrower_Id))
                .ForMember(d => d.BorrowerLastName, o => o.MapFrom(s => s.Borrower_LastName))
                .ForMember(d => d.BorrowerMemberNumber, o => o.MapFrom(s => s.Borrower_MemberNumber))
                .ForMember(d => d.CreatedUtc, o => o.MapFrom(s => s.CreatedUtc))
                .ForMember(d => d.ModifiedUtc, o => o.MapFrom(s => s.ModifiedUtc))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.OrganizationId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Version, o => o.MapFrom(s => s.Version));

            Mapper.CreateMap<OrderItemDto, OrderItemDoc>()
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
                .ForMember(d => d.ArticleId, o => o.MapFrom(s => s.ArticleId))
                // TODO: Map Availability
                //.ForMember(d => d.Availability, o => o.MapFrom(s => s.Availability))
                .ForMember(d => d.FromUtc, o => o.MapFrom(s => s.FromUtc))
                .ForMember(d => d.ItemTotal, o => o.MapFrom(s => s.Amount * s.Article.Price))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.OrderItemId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Article.Name))
                .ForMember(d => d.ToUtc, o => o.MapFrom(s => s.ToUtc))
                .ForMember(d => d.UnitPrice, o => o.MapFrom(s => s.Article.Price));

            Mapper.CreateMap<OrderDto, OrderDetailDoc>()
                .ForMember(d => d.BorrowerFirstName, o => o.MapFrom(s => s.Borrower_FirstName))
                .ForMember(d => d.BorrowerId, o => o.MapFrom(s => s.Borrower_Id))
                .ForMember(d => d.BorrowerLastName, o => o.MapFrom(s => s.Borrower_LastName))
                .ForMember(d => d.BorrowerMemberNumber, o => o.MapFrom(s => s.Borrower_MemberNumber))
                .ForMember(d => d.CreatedUtc, o => o.MapFrom(s => s.CreatedUtc))
                .ForMember(d => d.ModifiedUtc, o => o.MapFrom(s => s.ModifiedUtc))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.OrganizationId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Version, o => o.MapFrom(s => s.Version))
                .ForMember(d => d.TotalPrice, o => o.MapFrom(s => s.Items.Sum(i => i.Amount * i.Article.Price)))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));
        }
    }

    public class OrderDoc
    {
        public int OrderId { get; set; }
        public int Version { get; set; }
        public int OrganizationId { get; set; }

        public string Status { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? ModifiedUtc { get; set; }

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
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }

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