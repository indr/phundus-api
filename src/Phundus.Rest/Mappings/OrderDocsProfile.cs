namespace Phundus.Rest.Mappings
{
    using System.Linq;
    using AutoMapper;
    using ContentObjects;
    using Core.Shop.Queries;

    public class OrderDocsProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OrderDto, ContentObjects.Lessee>()
                .ForMember(d => d.LesseeId, o => o.MapFrom(s => s.Borrower_Id))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Borrower_FirstName + " " + s.Borrower_LastName))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Borrower_Street))
                .ForMember(d => d.Postcode, o => o.MapFrom(s => s.Borrower_Postcode))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Borrower_City))
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.Borrower_EmailAddress))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Borrower_MobilePhoneNumber))
                .ForMember(d => d.MemberNumber, o => o.MapFrom(s => s.Borrower_MemberNumber));

            Mapper.CreateMap<OrderDto, Order>()
                .ForMember(d => d.CreatedAtUtc, o => o.MapFrom(s => s.CreatedUtc))
                .ForMember(d => d.ModifiedAtUtc, o => o.MapFrom(s => s.ModifiedUtc))
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.LessorId, o => o.MapFrom(s => s.Lessor_LessorId))
                .ForMember(d => d.LessorName, o => o.MapFrom(s => s.Lessor_Name))
                .ForMember(d => d.Lessee, o => o.MapFrom(s => s));

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
    }
}