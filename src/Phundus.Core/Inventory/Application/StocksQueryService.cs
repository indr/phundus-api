namespace Phundus.Core.Inventory.Application
{
    using System.Collections.Generic;
    using Common.Port.Adapter.Persistence;
    using Data;

    public interface IStocksQueryService
    {
        IEnumerable<StockData> AllStocksByArticleId(int articleId);
    }

    public class StocksQueryService : NHibernateQueryServiceBase<StockData>, IStocksQueryService
    {
        public IEnumerable<StockData> AllStocksByArticleId(int articleId)
        {
            return Query.Where(p => p.ArticleId == articleId).List();
        }
    }
}