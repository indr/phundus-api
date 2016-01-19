namespace Phundus.IdentityAccess.Queries.ReadModels
{
    using System;
    using System.Linq;
    using System.Web.Configuration;
    using Common.Domain.Model;
    using Cqrs;
    using EventSourcedViewsUpdater;

    public class RelationshipsQuery : NHibernateReadModelBase<RelationshipViewRow>, IRelationshipQueries
    {
        public RelationshipDto ByMemberIdForOrganizationId(UserId memberId, Guid organizationId)
        {
            var result = QueryOver().Where(p =>
                p.OrganizationGuid == organizationId && p.UserGuid == memberId.Id).List().FirstOrDefault();

            if (result == null)
            {
                return new RelationshipDto
                {
                    OrganizationGuid = organizationId,
                    UserId = memberId.Id,
                    Status = RelationshipStatusDto.None,
                    Timestamp = DateTime.UtcNow
                };
            }

            var status = RelationshipStatusDto.None;
            if (result.Status == "application")
                status = RelationshipStatusDto.Application;
            else if (result.Status == "rejected")
                status = RelationshipStatusDto.Rejected;
            else if (result.Status == "member")
                status = RelationshipStatusDto.Member;

            return new RelationshipDto
            {
                OrganizationGuid = result.OrganizationGuid,
                UserId = result.UserGuid,
                Status = status,
                Timestamp = result.Timestamp
            };
        }
    }

    public class RelationshipsReadModelReader : ReadModelReaderBase//, IRelationshipQueries
    {
        public RelationshipDto ByMemberIdForOrganizationId(UserId memberId, Guid organizationId)
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