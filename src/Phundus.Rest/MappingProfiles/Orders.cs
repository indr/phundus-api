namespace Phundus.Rest.MappingProfiles
{
    using System.Linq;
    using AutoMapper;
    using ContentObjects;
    using Shop.Projections;

    public class Orders : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OrderData, OrderDetail>()
                .ForMember(d => d.CreatedAtUtc, o => o.MapFrom(s => s.CreatedAtUtc))
                .ForMember(d => d.ModifiedAtUtc, o => o.MapFrom(s => s.ModifiedAtUtc))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.OrderShortId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.LessorId, o => o.MapFrom(s => s.LessorId))
                .ForMember(d => d.LessorName, o => o.MapFrom(s => s.LessorName))
                .ForMember(d => d.Lessee, o => o.MapFrom(s => s))
                .ForMember(d => d.TotalPrice, o => o.MapFrom(s => s.Items.Sum(i => i.LineTotal)))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));

            Mapper.CreateMap<OrderData, Order>()
                .ForMember(d => d.CreatedAtUtc, o => o.MapFrom(s => s.CreatedAtUtc))
                .ForMember(d => d.ModifiedAtUtc, o => o.MapFrom(s => s.ModifiedAtUtc))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.OrderShortId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.LessorId, o => o.MapFrom(s => s.LessorId))
                .ForMember(d => d.LessorName, o => o.MapFrom(s => s.LessorName))
                .ForMember(d => d.Lessee, o => o.MapFrom(s => s));
        }
    }
}