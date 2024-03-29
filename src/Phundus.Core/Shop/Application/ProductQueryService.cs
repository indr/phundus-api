namespace Phundus.Shop.Application
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Common;
    using Common.Projections;
    using Common.Querying;
    using NHibernate.Criterion;
    using NHibernate.SqlCommand;

    public interface IProductQueryService
    {
        QueryResult<ProductListData> Query(string q, Guid? lessorId, int? offset, int? limit);
        ProductDetailsData Get(Guid itemGuid);
    }

    public class ProductQueryService : QueryServiceBase, IProductQueryService
    {
        private const int DefaultLimit = 10;

        public QueryResult<ProductListData> Query(string q, Guid? lessorId, int? offset, int? limit)
        {
            offset = offset ?? 0;
            limit = limit > 0 ? limit : DefaultLimit;

            ProductListData items = null;
            var query = Session.QueryOver(() => items);
            if (!String.IsNullOrWhiteSpace(q))
            {
                var d = new Disjunction();
                d.Add(Restrictions.On<ProductListData>(e => e.Name).IsInsensitiveLike(q, MatchMode.Anywhere));
                d.Add(Restrictions.On<ProductListData>(e => e.TagsAsString).IsInsensitiveLike(q, MatchMode.Anywhere));

                query.Where(d);
            }
            if (lessorId.HasValue)
            {
                query = query.Where(p => p.LessorId == lessorId);
            }

            ProductListPopularityData popularity = null;

            query.JoinAlias(() => items.Popularities, () => popularity, JoinType.LeftOuterJoin,
                Restrictions.Where<ProductListPopularityData>(p => p.Month == DateTime.Today.Month));


            query = query.OrderBy(() => popularity.Value).Desc.ThenBy(p => p.CreatedAtUtc).Desc;

            var total = query.RowCountInt64();
            var result = query.Skip(offset.Value).Take(limit.Value).List();
            return new QueryResult<ProductListData>(offset.Value, limit.Value, total, result);
        }

        public ProductDetailsData Get(Guid itemGuid)
        {
            var result = Session.QueryOver<ProductDetailsData>()
                .Where(p => p.ArticleId == itemGuid)
                .SingleOrDefault();
            if (result == null)
                throw new NotFoundException("Shop item {0} not found.", itemGuid);
            return result;
        }
    }
}