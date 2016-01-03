namespace Phundus.Rest.Api.Organizations
{
    using System;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using AutoMapper;
    using Castle.Transactions;
    using ContentObjects;
    using Core.IdentityAndAccess.Queries;

    [RoutePrefix("api/organizations/{organizationId}/relationships")]
    public class OrganizationsRelationshipsController : ApiControllerBase
    {
        static OrganizationsRelationshipsController()
        {
            Mapper.CreateMap<RelationshipDto, Relationship>()
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s => s.Timestamp))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        }

        public IRelationshipQueries RelationshipQueries { get; set; }

        [GET("")]
        [Transaction]
        public virtual Relationship Get(Guid organizationId)
        {
            return Map<Relationship>(RelationshipQueries.ByMemberIdForOrganizationId(CurrentUserId, organizationId));
        }
    }
}