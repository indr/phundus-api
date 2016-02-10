namespace Phundus.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Cqrs;
    using Inventory.Queries;
    using NHibernate.Criterion;
    using Projections;

    public interface IItemQueries
    {
        QueryResult<ResultItemsProjectionRow> Query(string q, Guid? lessorId, int offset, int limit);
        ShopItemProjectionRow Get(Guid itemGuid);
    }

    public class ItemQueriesReadModel : ReadModelBase, IItemQueries
    {
        public QueryResult<ResultItemsProjectionRow> Query(string q, Guid? lessorId, int offset, int limit)
        {
            limit = limit > 0 ? limit : 20;

            var query = QueryOver<ResultItemsProjectionRow>();
            if (!String.IsNullOrWhiteSpace(q))
            {
                q = q.ToLowerInvariant();
                query = query.WhereRestrictionOn(e => e.Name).IsLike(q, MatchMode.Anywhere);
            }
            if (lessorId.HasValue)
            {
                query = query.Where(p => p.OwnerGuid == lessorId);
            }

            query = query.OrderBy(p => p.CreatedAtUtc).Desc;

            var total = query.RowCountInt64();
            var result = query.Skip(offset).Take(limit).List();
            return new QueryResult<ResultItemsProjectionRow>(offset, limit, total, result);
        }

        public ShopItemProjectionRow Get(Guid itemGuid)
        {
            var result = Session.QueryOver<ShopItemProjectionRow>()
                .Where(p => p.ArticleGuid == itemGuid)
                .SingleOrDefault();
            if (result == null)
                throw new NotFoundException("Shop item {0} not found.", itemGuid);
            return result;
        }
    }
}