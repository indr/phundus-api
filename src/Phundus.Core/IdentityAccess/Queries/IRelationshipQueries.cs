namespace Phundus.IdentityAccess.Queries
{
    using System;
    using Common.Domain.Model;
    using Projections;

    public interface IRelationshipQueries
    {
        RelationshipProjectionRow ByMemberIdForOrganizationId(UserId memberId, Guid organizationId);
    }
}