namespace Phundus.Inventory.Application
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using Common.Querying;
    using NHibernate.Criterion;
    using Projections;

    public interface IArticleQueryService
    {
        ArticleData GetById(ArticleId articleId);

        IEnumerable<ArticleData> Query(InitiatorId initiatorId, OwnerId queryOwnerId, string query);
    }

    public class ArticleQueryService : QueryServiceBase<ArticleData>, IArticleQueryService
    {
        public ArticleData GetById(ArticleId articleId)
        {
            var result = Find(articleId);
            if (result == null)
                throw new NotFoundException(String.Format("Article {0} not found.", articleId));
            return result;
        }

        public IEnumerable<ArticleData> Query(InitiatorId initiatorId, OwnerId queryOwnerId, string query)
        {
            query = query == null ? "" : query.ToLowerInvariant();
            return QueryOver().Where(p => p.OwnerGuid == queryOwnerId.Id)
                .AndRestrictionOn(p => p.Name).IsInsensitiveLike(query, MatchMode.Anywhere)
                .List();
        }
    }
}