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
            Mapper.CreateMap<OrderData, Order>()
               .ForMember(d => d.CreatedAtUtc, o => o.MapFrom(s => s.CreatedAtUtc))
               .ForMember(d => d.ModifiedAtUtc, o => o.MapFrom(s => s.ModifiedAtUtc))
               .ForMember(d => d.OrderId, o => o.MapFrom(s => s.OrderShortId))
               .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
               .ForMember(d => d.LessorId, o => o.MapFrom(s => s.LessorId))
               .ForMember(d => d.LessorName, o => o.MapFrom(s => s.LessorName))
               .ForMember(d => d.Lessee, o => o.MapFrom(s => s));

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

            Mapper.CreateMap<OrderLineData, OrderItem>()
                .ForMember(d => d.Amount, o => o.MapFrom(s => s.Quantity))
                .ForMember(d => d.ArticleId, o => o.MapFrom(s => s.ArticleShortId))
                .ForMember(d => d.FromUtc, o => o.MapFrom(s => s.FromUtc))
                .ForMember(d => d.IsAvailable, o => o.MapFrom(s => s.IsAvailable))
                .ForMember(d => d.ItemTotal, o => o.MapFrom(s => s.LineTotal))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Order.OrderShortId))
                .ForMember(d => d.OrderItemId, o => o.MapFrom(s => s.LineId))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Text))
                .ForMember(d => d.ToUtc, o => o.MapFrom(s => s.ToUtc))
                .ForMember(d => d.UnitPrice, o => o.MapFrom(s => s.UnitPricePerWeek));

            Mapper.CreateMap<OrderData, Lessee>()
                .ForMember(d => d.City, o => o.MapFrom(s => s.LesseeCity))
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.LesseeEmailAddress))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.LesseeFirstName + " " + s.LesseeLastName))
                .ForMember(d => d.LesseeId, o => o.MapFrom(s => s.LesseeId))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.LesseePhoneNumber))
                .ForMember(d => d.Postcode, o => o.MapFrom(s => s.LesseePostcode))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.LesseeStreet));
        }
    }
}