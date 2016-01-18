namespace Phundus.IdentityAccess.Queries
{
    using System;
    using Common.Domain.Model;

    public interface IRelationshipQueries
    {
        RelationshipDto ByMemberIdForOrganizationId(UserId memberId, Guid organizationId);
    }
}