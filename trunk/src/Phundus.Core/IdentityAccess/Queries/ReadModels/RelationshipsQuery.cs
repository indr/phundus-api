namespace Phundus.IdentityAccess.Queries.ReadModels
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Cqrs;
    using EventSourcedViewsUpdater;

    public class RelationshipsQuery : NHibernateReadModelBase<RelationshipViewRow>, IRelationshipQueries
    {
        public RelationshipViewRow ByMemberIdForOrganizationId(UserId memberId, Guid organizationId)
        {
            var result = QueryOver().Where(p =>
                p.OrganizationGuid == organizationId && p.UserGuid == memberId.Id).SingleOrDefault();

            if (result != null)
                return result;

            return new RelationshipViewRow
            {
                OrganizationGuid = organizationId,
                UserGuid = memberId.Id,
                Status = null,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}