namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Core.Shop.Queries;

    public class OrganizationOrderDocsProfile : Profile
    {
        protected override void Configure()
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
    }

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