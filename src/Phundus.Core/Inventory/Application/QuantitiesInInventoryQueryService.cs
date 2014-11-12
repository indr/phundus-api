namespace Phundus.Core.Inventory.Application
{
    using System;
    using Common.Port.Adapter.Persistence;
    using Port.Adapter.Persistence.View;

    public interface IQuantitiesInInventoryQueryService
    {
        QuantitiesInInventoryData QuantityDataAsOf(DateTime asOfUtc);
    }

    public class QuantitiesInInventoryQueryService : NHibernateQueryServiceBase<QuantitiesInInventoryData>,
        IQuantitiesInInventoryQueryService
    {
        public QuantitiesInInventoryData QuantityDataAsOf(DateTime asOfUtc)
        {
            return Query().Where(p => p.AsOfUtc <= asOfUtc).OrderBy(p => p.AsOfUtc).Desc.Take(1).SingleOrDefault();
        }
    }
}