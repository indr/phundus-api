namespace Phundus.Shop.Queries
{
    using System.Collections.Generic;
    using Cqrs;

    public interface IItemQueries
    {
        IList<ShopArticleSearchResultDto> Query(string globalSearch);
    }

    public class ItemQueriesReadModel : ReadModelBase, IItemQueries
    {
        public IList<ShopArticleSearchResultDto> Query(string globalSearch)
        {
            return new List<ShopArticleSearchResultDto>();
        }
    }
}