namespace Phundus.IdentityAccess.Application
{
    using System;
    using Common.Domain.Model;
    using Common.Querying;

    public interface IRelationshipQueryService
    {
        RelationshipData ByMemberIdForOrganizationId(UserId memberId, Guid organizationId);
    }

    public class RelationshipQueryService : QueryServiceBase<RelationshipData>, IRelationshipQueryService
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

    public class RelationshipData
    {
        public virtual Guid RowGuid { get; set; }
        public virtual Guid OrganizationGuid { get; set; }
        public virtual Guid UserGuid { get; set; }
        public virtual DateTime Timestamp { get; set; }
        public virtual string Status { get; set; }
    }
}