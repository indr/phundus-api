namespace Phundus.Core.Inventory.Application
{
    using System.Collections.Generic;
    using Common.Port.Adapter.Persistence;
    using Data;

    public interface IQuantitesAvailableQueryService
    {
        IEnumerable<QuantityAvailableData> AllQuantitiesAvailableByArticleId(int organizationId, int articleId,
            string stockId);
    }

    public class QuantitiesAvailableQueryService : NHibernateQueryServiceBase<QuantityAvailableData>,
        IQuantitesAvailableQueryService
    {
        public IEnumerable<QuantityAvailableData> AllQuantitiesAvailableByArticleId(int organizationId, int articleId,
            string stockId)
        {
            return Query.Where(p => p.StockId == stockId).List();
        }
    }
}