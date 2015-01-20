namespace Phundus.Core.Inventory.Application
{
    using System.Collections.Generic;
    using Common.Port.Adapter.Persistence;
    using Data;

    public interface IQuantitiesAvailableQueryService
    {
        IEnumerable<QuantityAvailableData> AllQuantitiesAvailableByArticleId(int organizationId, int articleId);
        IEnumerable<QuantityAvailableData> AllQuantitiesAvailableByStockId(int organizationId, int articleId, string stockId);
    }

    public class QuantitiesAvailableQueryService : NHibernateQueryServiceBase<QuantityAvailableData>,
        IQuantitiesAvailableQueryService
    {
        public IEnumerable<QuantityAvailableData> AllQuantitiesAvailableByArticleId(int organizationId, int articleId)
        {
            return Query.Where(p => p.OrganizationId == organizationId).And(p => p.ArticleId == articleId).OrderBy(p => p.AsOfUtc).Asc.List();
        }

        public IEnumerable<QuantityAvailableData> AllQuantitiesAvailableByStockId(int organizationId, int articleId,
            string stockId)
        {
            return Query.Where(p => p.OrganizationId == organizationId).And(p => p.ArticleId == articleId).And(p => p.StockId == stockId)
                .OrderBy(p => p.AsOfUtc).Asc.List();
        }
    }
}