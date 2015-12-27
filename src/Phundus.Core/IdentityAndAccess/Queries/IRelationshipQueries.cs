namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;

    public interface IRelationshipQueries
    {
        RelationshipDto ByMemberIdForOrganizationId(int memberId, Guid organizationId);
    }
}