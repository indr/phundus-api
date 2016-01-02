namespace Phundus.Rest.Api.Docs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core.Shop.Queries;
    using Newtonsoft.Json;

    public class OrderDocsProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OrderDto, OrderDoc>()
                .ForMember(d => d.CreatedAtUtc, o => o.MapFrom(s => s.CreatedUtc))
                .ForMember(d => d.ModifiedAtUtc, o => o.MapFrom(s => s.ModifiedUtc))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.LessorId, o => o.MapFrom(s => s.Lessor_LessorId))
                .ForMember(d => d.LessorName, o => o.MapFrom(s => s.Lessor_Name))
                .ForMember(d => d.LesseeId, o => o.MapFrom(s => s.Borrower_Id))
                .ForMember(d => d.LesseeName, o => o.MapFrom(s => s.Borrower_FirstName + " " + s.Borrower_LastName));

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
                .ForMember(d => d.CreatedAtUtc, o => o.MapFrom(s => s.CreatedUtc))
                .ForMember(d => d.ModifiedAtUtc, o => o.MapFrom(s => s.ModifiedUtc))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.LessorId, o => o.MapFrom(s => s.Lessor_LessorId))
                .ForMember(d => d.LessorName, o => o.MapFrom(s => s.Lessor_Name))
                .ForMember(d => d.LesseeId, o => o.MapFrom(s => s.Borrower_Id))
                .ForMember(d => d.LesseeName, o => o.MapFrom(s => s.Borrower_FirstName + " " + s.Borrower_LastName))
                .ForMember(d => d.TotalPrice, o => o.MapFrom(s => s.Items.Sum(i => i.ItemTotal)))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));
        }
    }

    public class OrderDoc
    {
        [JsonProperty("orderId")]
        public int OrderId { get; set; }

        [JsonProperty("createdAtUtc")]
        public DateTime CreatedAtUtc { get; set; }

        [JsonProperty("modifiedAtUtc")]
        public DateTime? ModifiedAtUtc { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("lessorId")]
        public Guid LessorId { get; set; }

        [JsonProperty("lessorName")]
        public string LessorName { get; set; }

        [JsonProperty("lesseeId")]
        public int LesseeId { get; set; }

        [JsonProperty("lesseeName")]
        public string LesseeName { get; set; }
    }

    public class OrderDetailDoc : OrderDoc
    {
        private IList<OrderItemDoc> _items = new List<OrderItemDoc>();

        [JsonProperty("items")]
        public IList<OrderItemDoc> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        [JsonProperty("totalPrice")]
        public decimal TotalPrice { get; set; }
    }

    public class OrderItemDoc
    {
        public int OrderId { get; set; }
        public Guid OrderItemId { get; set; }

        public int ArticleId { get; set; }
        public string Text { get; set; }

        public bool IsAvailable { get; set; }
        public int Amount { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal ItemTotal { get; set; }
    }
}