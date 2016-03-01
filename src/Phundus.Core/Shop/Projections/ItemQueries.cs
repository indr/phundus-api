namespace Phundus.Shop.Projections
{
    using System;
    using Common;
    using Common.Projections;
    using Common.Querying;
    using Cqrs;
    using NHibernate.Criterion;
    using NHibernate.SqlCommand;

    public interface IItemQueries
    {
        QueryResult<ShopItemsData> Query(string q, Guid? lessorId, int? offset, int? limit);
        ShopItemData Get(Guid itemGuid);
    }

    public class ItemQueries : QueryBase, IItemQueries
    {
        private const int DefaultLimit = 10;

        public QueryResult<ShopItemsData> Query(string q, Guid? lessorId, int? offset, int? limit)
        {
            offset = offset ?? 0;
            limit = limit > 0 ? limit : DefaultLimit;

            ShopItemsData items = null;
            var query = Session.QueryOver(() => items);
            if (!String.IsNullOrWhiteSpace(q))
            {
                q = q.ToLowerInvariant();
                query = query.WhereRestrictionOn(e => e.Name).IsLike(q, MatchMode.Anywhere);
            }
            if (lessorId.HasValue)
            {
                query = query.Where(p => p.LessorId == lessorId);
            }

            ShopItemsPopularityData popularity = null;

            query.JoinAlias(() => items.Popularities, () => popularity, JoinType.LeftOuterJoin,
                Restrictions.Where<ShopItemsPopularityData>(p => p.Month == DateTime.Today.Month));


            query = query.OrderBy(() => popularity.Value).Desc.ThenBy(p => p.CreatedAtUtc).Desc;

            var total = query.RowCountInt64();
            var result = query.Skip(offset.Value).Take(limit.Value).List();
            return new QueryResult<ShopItemsData>(offset.Value, limit.Value, total, result);
        }

        public ShopItemData Get(Guid itemGuid)
        {
            var result = Session.QueryOver<ShopItemData>()
                .Where(p => p.ArticleId == itemGuid)
                .SingleOrDefault();
            if (result == null)
                throw new NotFoundException("Shop item {0} not found.", itemGuid);
            return result;
        }
    }
}