namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using IdentityAccess.Queries;

    public interface IRelationshipQueries
    {
        RelationshipDto ByMemberIdForOrganizationId(int memberId, Guid organizationId);
    }
}