namespace Phundus.Core.Inventory.Application
{
    using System;
    using System.Collections.Generic;
    using Common.Port.Adapter.Persistence;
    using Data;

    public interface IQuantitiesInInventoryQueryService
    {
        QuantityInInventoryData QuantityDataAsOf(DateTime asOfUtc);
        IEnumerable<QuantityInInventoryData> AllQuantitiesInInventoryByArticleId();
    }

    public class QuantitiesInInventoryQueryService : NHibernateQueryServiceBase<QuantityInInventoryData>,
        IQuantitiesInInventoryQueryService
    {
        public QuantityInInventoryData QuantityDataAsOf(DateTime asOfUtc)
        {
            return Query.Where(p => p.AsOfUtc <= asOfUtc).OrderBy(p => p.AsOfUtc).Desc.Take(1).SingleOrDefault();
        }

        public IEnumerable<QuantityInInventoryData> AllQuantitiesInInventoryByArticleId()
        {
            return Query.OrderBy(p => p.AsOfUtc).Asc.List();
        }
    }
}