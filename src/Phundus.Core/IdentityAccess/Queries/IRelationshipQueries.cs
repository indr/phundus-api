namespace Phundus.IdentityAccess.Queries
{
    using System;
    using Common.Domain.Model;

    public interface IRelationshipQueries
    {
        RelationshipDto ByMemberIdForOrganizationId(UserGuid memberId, Guid organizationId);
    }
}