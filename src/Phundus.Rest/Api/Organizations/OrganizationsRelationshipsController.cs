namespace Phundus.Rest.Api.Organizations
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common;
    using ContentObjects;
    using IdentityAccess.Queries;
    using Newtonsoft.Json;

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
            var result = _relationshipQueries.ByMemberIdForOrganizationId(CurrentUserId, organizationId);
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