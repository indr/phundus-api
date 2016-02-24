﻿namespace Phundus.IdentityAccess.Queries.ReadModels
{
    using System;
    using Common.Domain.Model;
    using Cqrs;
    using Projections;

    public class RelationshipsQuery : NHibernateReadModelBase<RelationshipProjectionRow>, IRelationshipQueries
    {
        public RelationshipProjectionRow ByMemberIdForOrganizationId(UserId memberId, Guid organizationId)
        {
            var result = QueryOver().Where(p =>
                p.OrganizationGuid == organizationId && p.UserGuid == memberId.Id).SingleOrDefault();

            if (result != null)
                return result;

            return new RelationshipProjectionRow
            {
                OrganizationGuid = organizationId,
                UserGuid = memberId.Id,
                Status = null,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}