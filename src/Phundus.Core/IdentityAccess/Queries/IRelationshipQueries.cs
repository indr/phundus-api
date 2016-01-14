namespace Phundus.IdentityAccess.Queries
{
    using System;

    public interface IRelationshipQueries
    {
        RelationshipDto ByMemberIdForOrganizationId(int memberId, Guid organizationId);
    }
}