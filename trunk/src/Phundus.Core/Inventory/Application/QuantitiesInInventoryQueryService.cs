namespace Phundus.Core.Inventory.Application
{
    using System;
    using System.Collections.Generic;
    using Common.Port.Adapter.Persistence;
    using Data;
    using NHibernate;

    public interface IQuantitiesInInventoryQueryService
    {
        QuantityInInventoryData QuantityDataAsOf(int organizationId, int articleId, string stockId, DateTime asOfUtc);

        IEnumerable<QuantityInInventoryData> AllQuantitiesInInventoryByArticleId(int organizationId, int articleId,
            string stockId);
    }

    public class QuantitiesInInventoryQueryService : NHibernateQueryServiceBase<QuantityInInventoryData>,
        IQuantitiesInInventoryQueryService
    {
        public QuantityInInventoryData QuantityDataAsOf(int organizationId, int articleId, string stockId,
            DateTime asOfUtc)
        {
            return QueryWhere(organizationId, articleId, stockId)
                .And(p => p.AsOfUtc <= asOfUtc)
                .OrderBy(p => p.AsOfUtc)
                .Desc.Take(1)
                .SingleOrDefault();
        }

        public IEnumerable<QuantityInInventoryData> AllQuantitiesInInventoryByArticleId(int organizationId,
            int articleId, string stockId)
        {
            return QueryWhere(organizationId, articleId, stockId)
                .OrderBy(p => p.AsOfUtc).Asc.List();
        }

        private IQueryOver<QuantityInInventoryData, QuantityInInventoryData> QueryWhere(int organizationId,
            int articleId, string stockId)
        {
            return Query.Where(p => p.OrganizationId == organizationId)
                .And(p => p.ArticleId == articleId)
                .And(p => p.StockId == stockId);
        }
    }
}