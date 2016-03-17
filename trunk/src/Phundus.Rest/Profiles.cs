namespace Phundus.Rest.MappingProfiles
{
    using System.Collections.Generic;
    using System.Linq;
    using Api;
    using AutoMapper;
    using ContentObjects;
    using IdentityAccess.Application;
    using Inventory.Application;
    using Inventory.Projections;
    using Shop.Application;

    public class OrganizationsProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OrganizationData, OrganizationsGetOkResponseContent>()
                .ForMember(d => d.Contact, mo => mo.MapFrom(s => s));
            Mapper.CreateMap<IList<StoreDetailsData>, OrganizationsGetOkResponseContent>()
                .ForMember(d => d.Stores, mo => mo.MapFrom(s => s));
            Mapper.CreateMap<OrganizationData, ContactCto>();
        }
    }

    public class StoresProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<StoreListData, StoreDetailsCto>();
            Mapper.CreateMap<StoreDetailsData, StoreDetailsCto>()
                .ForMember(d => d.Contact, mo => mo.MapFrom(s => s))
                .ForMember(d => d.Coordinate, mo => mo.MapFrom(s => s));
            Mapper.CreateMap<StoreDetailsData, ContactCto>();
            Mapper.CreateMap<StoreDetailsData, CoordinateCto>()
                .ForAllMembers(mo => mo.Condition(s => s.Latitude.HasValue && s.Longitude.HasValue));
        }
    }


    public class Orders : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OrderData, Order>()
                .ForMember(d => d.CreatedAtUtc, o => o.MapFrom(s => s.CreatedAtUtc))
                .ForMember(d => d.ModifiedAtUtc, o => o.MapFrom(s => s.ModifiedAtUtc))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.OrderShortId, o => o.MapFrom(s => s.OrderShortId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.LessorId, o => o.MapFrom(s => s.LessorId))
                .ForMember(d => d.LessorName, o => o.MapFrom(s => s.LessorName))
                .ForMember(d => d.Lessee, o => o.MapFrom(s => s));

            Mapper.CreateMap<OrderData, OrderDetail>()
                .ForMember(d => d.CreatedAtUtc, o => o.MapFrom(s => s.CreatedAtUtc))
                .ForMember(d => d.ModifiedAtUtc, o => o.MapFrom(s => s.ModifiedAtUtc))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.OrderShortId, o => o.MapFrom(s => s.OrderShortId))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.LessorId, o => o.MapFrom(s => s.LessorId))
                .ForMember(d => d.LessorName, o => o.MapFrom(s => s.LessorName))
                .ForMember(d => d.Lessee, o => o.MapFrom(s => s))
                .ForMember(d => d.TotalPrice, o => o.MapFrom(s => s.Lines.Sum(i => i.LineTotal)))
                .ForMember(d => d.Items, o => o.MapFrom(s => s.Lines));

            Mapper.CreateMap<OrderLineData, OrderItem>()
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Order.OrderId))
                .ForMember(d => d.OrderShortId, o => o.MapFrom(s => s.Order.OrderShortId))
                .ForMember(d => d.OrderItemId, o => o.MapFrom(s => s.LineId))
                .ForMember(d => d.ArticleId, o => o.MapFrom(s => s.ArticleId))
                .ForMember(d => d.ArticleShortId, o => o.MapFrom(s => s.ArticleShortId))
                .ForMember(d => d.IsAvailable, o => o.MapFrom(s => s.IsAvailable))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Text))
                .ForMember(d => d.FromUtc, o => o.MapFrom(s => s.FromUtc))
                .ForMember(d => d.ToUtc, o => o.MapFrom(s => s.ToUtc))
                .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity))
                .ForMember(d => d.UnitPrice, o => o.MapFrom(s => s.UnitPricePerWeek))
                .ForMember(d => d.LineTotal, o => o.MapFrom(s => s.LineTotal));

            Mapper.CreateMap<OrderData, LesseeCto>()
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