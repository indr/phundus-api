namespace Phundus.IdentityAccess.Queries
{
    using System;
    using Common.Domain.Model;
    using EventSourcedViewsUpdater;

    public interface IRelationshipQueries
    {
        RelationshipViewRow ByMemberIdForOrganizationId(UserId memberId, Guid organizationId);
    }
}