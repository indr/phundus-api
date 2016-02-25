namespace Phundus.Shop.Queries
{
    using System;
    using Common;
    using Common.Projections;
    using Cqrs;
    using NHibernate.Criterion;
    using NHibernate.SqlCommand;
    using Projections;

    public interface IItemQueries
    {
        QueryResult<ResultItemsProjectionRow> Query(string q, Guid? lessorId, int? offset, int? limit);
        ShopItemProjectionRow Get(Guid itemGuid);
    }

    public class ItemQueriesReadModel : ReadModelBase, IItemQueries
    {
        private const int DefaultLimit = 10;

        public QueryResult<ResultItemsProjectionRow> Query(string q, Guid? lessorId, int? offset, int? limit)
        {
            offset = offset ?? 0;
            limit = limit > 0 ? limit : DefaultLimit;

            ResultItemsProjectionRow item = null;
            var query = Session.QueryOver(() => item);
            if (!String.IsNullOrWhiteSpace(q))
            {
                q = q.ToLowerInvariant();
                query = query.WhereRestrictionOn(e => e.Name).IsLike(q, MatchMode.Anywhere);
            }
            if (lessorId.HasValue)
            {
                query = query.Where(p => p.OwnerGuid == lessorId);
            }

            ShopItemsSortByPopularityProjectionRow popularity = null;

            query.JoinAlias(() => item.Popularities, () => popularity, JoinType.LeftOuterJoin,
                Restrictions.Where<ShopItemsSortByPopularityProjectionRow>(p => p.Month == DateTime.Today.Month));


            query = query.OrderBy(() => popularity.Value).Desc.ThenBy(p => p.CreatedAtUtc).Desc;

            var total = query.RowCountInt64();
            var result = query.Skip(offset.Value).Take(limit.Value).List();
            return new QueryResult<ResultItemsProjectionRow>(offset.Value, limit.Value, total, result);
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