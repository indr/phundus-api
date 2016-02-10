namespace Phundus.Shop.Queries
{
    using System;
    using Cqrs;
    using Cqrs.Paging;
    using Projections;

    public interface IShopArticleQueries
    {
        PagedResult<ResultItemsProjectionRow> FindArticles(PageRequest pageRequest, string query, Guid? organization);
    }

    public class ShopArticleReadModel : ReadModelBase, IShopArticleQueries
    {
        private readonly IItemQueries _itemQueries;

        public ShopArticleReadModel(IItemQueries itemQueries)
        {
            if (itemQueries == null) throw new ArgumentNullException("itemQueries");
            _itemQueries = itemQueries;
        }

        public PagedResult<ResultItemsProjectionRow> FindArticles(PageRequest pageRequest, string query,
            Guid? organization)
        {
            var result = _itemQueries.Query(query, organization, pageRequest.Offset, pageRequest.Size);

            var pageResponse = new PageResponse
            {
                Size = result.Limit,
                Total = result.Total,
                Index = (int) Math.Floor((result.Offset + 1)/(double) result.Limit)
            };

            return new PagedResult<ResultItemsProjectionRow>(pageResponse, result.Result);
        }
    }
}