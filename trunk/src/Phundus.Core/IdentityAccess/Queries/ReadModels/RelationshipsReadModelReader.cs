namespace Phundus.IdentityAccess.Queries.ReadModels
{
    using System;
    using System.Linq;
    using Common.Domain.Model;

    public class RelationshipsReadModelReader : ReadModelReaderBase, IRelationshipQueries
    {
        public RelationshipDto ByMemberIdForOrganizationId(UserGuid memberId, Guid organizationId)
        {
            var result = (from r in Ctx.RelationshipDtos
                where r.OrganizationGuid == organizationId && r.UserId == memberId.Id
                select r).FirstOrDefault();

            if (result != null)
                return result;

            return new RelationshipDto
            {
                OrganizationGuid = organizationId,
                UserId = memberId.Id,
                Status = RelationshipStatusDto.None,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}