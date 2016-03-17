namespace Phundus.Rest.Api.Organizations
{
    using System;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Resources;
    using ContentObjects;
    using IdentityAccess.Application;
    using Newtonsoft.Json;

    [RoutePrefix("api/organizations/{organizationId}/relationships")]
    public class OrganizationsRelationshipsController : ApiControllerBase
    {
        private readonly IRelationshipQueryService _relationshipQueryService;

        public OrganizationsRelationshipsController(IRelationshipQueryService relationshipQueryService)
        {
            _relationshipQueryService = relationshipQueryService;
        }

        [GET("")]
        [Transaction]
        public virtual OrganizationsRelationshipsQueryOkResponseContent Get(Guid organizationId)
        {
            var result = _relationshipQueryService.ByMemberIdForOrganizationId(CurrentUserId, organizationId);
            return new OrganizationsRelationshipsQueryOkResponseContent
            {
                Result = new Relationship
                {
                    OrganizationId = result.OrganizationGuid,
                    Status = result.Status,
                    Timestamp = result.Timestamp,
                    UserId = result.UserGuid
                }
            };
        }
    }

    public class OrganizationsRelationshipsQueryOkResponseContent
    {
        [JsonProperty("result")]
        public Relationship Result { get; set; }
    }
}