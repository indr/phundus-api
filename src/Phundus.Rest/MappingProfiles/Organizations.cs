namespace Phundus.Rest.MappingProfiles
{
    using Api.Organizations;
    using AutoMapper;
    using IdentityAccess.Queries;

    public class Organizations : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<RelationshipDto, OrganizationsRelationshipsQueryOkResponseContent>()
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s => s.Timestamp))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        }
    }
}