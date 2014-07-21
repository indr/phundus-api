namespace phiNdus.fundus.Web.App_Start
{
    using AutoMapper;
    using Phundus.Core.OrganisationCtx.DomainModel;
    using Phundus.Rest.Dtos;

    public class AutoMapperConfig
    {
        public static void Config()
        {
            //Mapper.CreateMap<Membership, MemberDto>()
            //    .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            //    .ForMember(d => d.MemberVersion, o => o.MapFrom(s => s.User.Version))
            //    .ForMember(d => d.MembershipVersion, o => o.MapFrom(s => s.Version))
            //    .ForMember(d => d.FirstName, o => o.MapFrom(s => s.User.FirstName))
            //    .ForMember(d => d.LastName, o => o.MapFrom(s => s.User.LastName))
            //    .ForMember(d => d.JsNumber, o => o.MapFrom(s => s.User.JsNumber))
            //    .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.User.Membership.Email))
            //    .ForMember(d => d.Role, o => o.MapFrom(s => s.Role))
            //    .ForMember(d => d.RequestDate, o => o.MapFrom(s => s.RequestDate))
            //    .ForMember(d => d.IsLockedOut, o => o.MapFrom(s => s.IsLockedOut))
            //    .ForMember(d => d.LastLockoutDate, o => o.MapFrom(s => s.LastLockoutDate))
            //    .ForMember(d => d.IsApproved, o => o.MapFrom(s => s.IsApproved))
            //    .ForMember(d => d.ApprovalDate, o => o.MapFrom(s => s.ApprovalDate));

            Mapper.AssertConfigurationIsValid();
        }
    }
}