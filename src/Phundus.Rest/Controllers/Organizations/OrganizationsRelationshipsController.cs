namespace Phundus.Rest.Controllers.Organizations
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
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.OrganizationId))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s => s.Timestamp))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        }

        public IRelationshipQueries RelationshipQueries { get; set; }

        [GET("")]
        [Transaction]
        public virtual RelationshipDoc Get(int organizationId)
        {
            return Map<RelationshipDoc>(RelationshipQueries.ByMemberIdForOrganizationId(CurrentUserId, organizationId));
        }
    }

    public class RelationshipDoc
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
}