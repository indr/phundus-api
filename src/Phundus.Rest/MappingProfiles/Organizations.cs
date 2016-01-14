namespace Phundus.Rest.MappingProfiles
{
    using Api.Organizations;
    using AutoMapper;
    using ContentObjects;
    using IdentityAccess.Queries;
    using IdentityAccess.Queries.ReadModels;

    public class Organizations : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<OrganizationDto, Organization>()
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.Guid))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.Url));

            Mapper.CreateMap<RelationshipDto, OrganizationsRelationshipsQueryOkResponseContent>()
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s => s.Timestamp))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        }
    }
}