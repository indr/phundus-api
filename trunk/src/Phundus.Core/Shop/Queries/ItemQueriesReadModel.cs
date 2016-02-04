namespace Phundus.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using Cqrs;
    using NHibernate;

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