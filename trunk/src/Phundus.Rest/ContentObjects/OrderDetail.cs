namespace Phundus.Rest.ContentObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core.Shop.Queries;
    using Newtonsoft.Json;

    public class OrderDetail : Order
    {
        static OrderDetail()
        {
            Mapper.CreateMap<OrderDto, OrderDetail>()
                .ForMember(d => d.CreatedAtUtc, o => o.MapFrom(s => s.CreatedUtc))
                .ForMember(d => d.ModifiedAtUtc, o => o.MapFrom(s => s.ModifiedUtc))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.LessorId, o => o.MapFrom(s => s.Lessor_LessorId))
                .ForMember(d => d.LessorName, o => o.MapFrom(s => s.Lessor_Name))
                .ForMember(d => d.Lessee, o => o.MapFrom(s => s))
                .ForMember(d => d.TotalPrice, o => o.MapFrom(s => s.Items.Sum(i => i.ItemTotal)))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));
        }

        private IList<OrderItem> _items = new List<OrderItem>();

        [JsonProperty("items")]
        public IList<OrderItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        [JsonProperty("totalPrice")]
        public decimal TotalPrice { get; set; }
    }
}