namespace Phundus.Rest.ContentObjects
{
    using System;
    using AutoMapper;
    using Core.Shop.Queries;
    using Newtonsoft.Json;

    public class OrderItem
    {
        static OrderItem()
        {
            Mapper.CreateMap<OrderItemDto, OrderItem>()
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
        }

        [JsonProperty("orderId")]
        public int OrderId { get; set; }

        [JsonProperty("orderItemId")]
        public Guid OrderItemId { get; set; }

        [JsonProperty("articleId")]
        public int ArticleId { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("fromUtc")]
        public DateTime FromUtc { get; set; }

        [JsonProperty("toUtc")]
        public DateTime ToUtc { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("itemTotal")]
        public decimal ItemTotal { get; set; }
    }
}