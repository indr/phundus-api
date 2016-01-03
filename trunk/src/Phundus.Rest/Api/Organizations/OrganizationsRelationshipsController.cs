namespace Phundus.Rest.Api.Organizations
{
    using System;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using AutoMapper;
    using Castle.Transactions;
    using Common;
    using ContentObjects;
    using Core.IdentityAndAccess.Queries;

    [RoutePrefix("api/organizations/{organizationId}/relationships")]
    public class OrganizationsRelationshipsController : ApiControllerBase
    {
        private readonly IRelationshipQueries _relationshipQueries;

        public OrganizationsRelationshipsController(IRelationshipQueries relationshipQueries)
        {
            AssertionConcern.AssertArgumentNotNull(relationshipQueries, "RelationshipQueries must be provided.");

            _relationshipQueries = relationshipQueries;
        }

        [GET("")]
        [Transaction]
        public virtual OrganizationsRelationshipsQueryOkResponseContent Get(Guid organizationId)
        {
            var result = _relationshipQueries.ByMemberIdForOrganizationId(CurrentUserId.Id, organizationId);
            return Map<OrganizationsRelationshipsQueryOkResponseContent>(result);
        }
    }

    public class OrganizationsRelationshipsQueryOkResponseContent : Relationship
    {
        static OrganizationsRelationshipsQueryOkResponseContent()
        {
            Mapper.CreateMap<RelationshipDto, OrganizationsRelationshipsQueryOkResponseContent>()
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s => s.Timestamp))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        }
    }
}