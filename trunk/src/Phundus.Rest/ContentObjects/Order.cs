namespace Phundus.Rest.ContentObjects
{
    using System;
    using AutoMapper;
    using Core.Shop.Queries;
    using Newtonsoft.Json;

    public class Order
    {
        static Order()
        {
            Mapper.CreateMap<OrderDto, Order>()
                .ForMember(d => d.CreatedAtUtc, o => o.MapFrom(s => s.CreatedUtc))
                .ForMember(d => d.ModifiedAtUtc, o => o.MapFrom(s => s.ModifiedUtc))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.LessorId, o => o.MapFrom(s => s.Lessor_LessorId))
                .ForMember(d => d.LessorName, o => o.MapFrom(s => s.Lessor_Name))
                .ForMember(d => d.Lessee, o => o.MapFrom(s => s));
        }

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

        [JsonProperty("lessee")]
        public Lessee Lessee { get; set; }
    }
}