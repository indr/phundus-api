namespace Phundus.Rest.Api.Organizations
{
    using System;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using AutoMapper;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;

    [RoutePrefix("api/organizations/{organizationId}/relationships")]
    public class OrganizationsRelationshipsController : ApiControllerBase
    {
        static OrganizationsRelationshipsController()
        {
            Mapper.CreateMap<RelationshipDto, RelationshipDoc>()
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s => s.Timestamp))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        }

        public IRelationshipQueries RelationshipQueries { get; set; }

        [GET("")]
        [Transaction]
        public virtual RelationshipDoc Get(Guid organizationId)
        {
            return Map<RelationshipDoc>(RelationshipQueries.ByMemberIdForOrganizationId(CurrentUserId, organizationId));
        }
    }

    public class RelationshipDoc
    {
        public Guid OrganizationId { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
}