namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Linq;

    public class RelationshipsReadModelReader : ReadModelReaderBase, IRelationshipQueries
    {
        public RelationshipDto ByMemberIdForOrganizationId(int memberId, int organizationId)
        {
            var result = (from r in Ctx.RelationshipDtos
                where r.OrganizationId == organizationId && r.UserId == memberId
                select r).FirstOrDefault();

            if (result != null)
                return result;

            return new RelationshipDto
            {
                OrganizationId = organizationId,
                UserId = memberId,
                Status = RelationshipStatusDto.None,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}