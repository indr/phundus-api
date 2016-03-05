namespace Phundus.IdentityAccess.Projections
{
    using System;
    using Common.Domain.Model;
    using Common.Querying;

    public interface IRelationshipQueries
    {
        RelationshipData ByMemberIdForOrganizationId(UserId memberId, Guid organizationId);
    }

    public class RelationshipQueries : QueryBase<RelationshipData>, IRelationshipQueries
    {
        public RelationshipData ByMemberIdForOrganizationId(UserId memberId, Guid organizationId)
        {
            var result = QueryOver().Where(p =>
                p.OrganizationGuid == organizationId && p.UserGuid == memberId.Id).SingleOrDefault();

            if (result != null)
                return result;

            return new RelationshipData
            {
                OrganizationGuid = organizationId,
                UserGuid = memberId.Id,
                Status = null,
                Timestamp = DateTime.UtcNow
            };
        }

    }
}