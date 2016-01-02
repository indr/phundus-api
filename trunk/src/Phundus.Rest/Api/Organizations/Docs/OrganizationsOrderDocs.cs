namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core.Shop.Queries;
    using Newtonsoft.Json;

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
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.Lessor_LessorId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Version, o => o.MapFrom(s => s.Version));

            Mapper.CreateMap<OrderItemDto, OrderItemDoc>()
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.Amount))
                .ForMember(d => d.ArticleId, o => o.MapFrom(s => s.ArticleId))
                .ForMember(d => d.IsAvailable, o => o.MapFrom(s => s.IsAvailable))
                .ForMember(d => d.FromUtc, o => o.MapFrom(s => s.FromUtc))
                .ForMember(d => d.ItemTotal, o => o.MapFrom(s => s.ItemTotal))
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
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.Lessor_LessorId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Version, o => o.MapFrom(s => s.Version))
                .ForMember(d => d.TotalPrice, o => o.MapFrom(s => s.Items.Sum(i => i.ItemTotal)))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));
        }
    }

    public class OrderDoc
    {
        public int OrderId { get; set; }
        public int Version { get; set; }
        public Guid OrganizationId { get; set; }

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
        public bool IsAvailable { get; set; }
    }

    public class OrderPatchDoc
    {
        public string Status { get; set; }
    }

    public class OrdersPostRequestContent
    {
        [JsonProperty("ownerId")]
        public Guid OwnerId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class OrderItemPostDoc
    {
        public int ArticleId { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public int Amount { get; set; }
    }

    public class OrderItemPatchDoc
    {
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public int Amount { get; set; }
        public decimal ItemTotal { get; set; }
    }
}