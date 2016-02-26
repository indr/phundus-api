namespace Phundus.Shop.Projections
{
    using System;
    using Common;
    using Common.Projections;
    using Cqrs;
    using NHibernate.Criterion;
    using NHibernate.SqlCommand;

    public interface IItemQueries
    {
        QueryResult<ShopItemData> Query(string q, Guid? lessorId, int? offset, int? limit);
        ShopItemDetailData Get(Guid itemGuid);
    }

    public class ItemQueriesReadModel : ProjectionBase, IItemQueries
    {
        private const int DefaultLimit = 10;

        public QueryResult<ShopItemData> Query(string q, Guid? lessorId, int? offset, int? limit)
        {
            offset = offset ?? 0;
            limit = limit > 0 ? limit : DefaultLimit;

            ShopItemData item = null;
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
            return new QueryResult<ShopItemData>(offset.Value, limit.Value, total, result);
        }

        public ShopItemDetailData Get(Guid itemGuid)
        {
            var result = Session.QueryOver<ShopItemDetailData>()
                .Where(p => p.ArticleGuid == itemGuid)
                .SingleOrDefault();
            if (result == null)
                throw new NotFoundException("Shop item {0} not found.", itemGuid);
            return result;
        }
    }
}