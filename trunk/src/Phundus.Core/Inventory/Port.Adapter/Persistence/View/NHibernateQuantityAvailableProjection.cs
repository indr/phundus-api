namespace Phundus.Core.Inventory.Port.Adapter.Persistence.View
{
    using System;
    using Application.Data;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence;
    using Domain.Model.Management;

    public class NHibernateQuantityAvailableProjection : NHibernateProjectionBase<QuantityAvailableData>,
        IDomainEventHandler
    {
        public void Handle(IDomainEvent e)
        {
            Process((dynamic) e);
        }

        private void Process(DomainEvent e)
        {
            // Fallback
        }

        private void Process(QuantityAvailableChanged e)
        {
            ProcessTo(e.StockId, e.ToUtc, e.Change);

            ProcessFrom(e.StockId, e.FromUtc, e.Change);

            ProcessInPeriod(e.StockId, e.FromUtc, e.ToUtc, e.Change);
        }

        private void ProcessInPeriod(string stockId, DateTime fromUtc, DateTime toUtc, int change)
        {
            var records =
                Query.Where(p => p.StockId == stockId).And(p => p.AsOfUtc > fromUtc).And(p => p.AsOfUtc < toUtc).List();
            foreach (var each in records)
            {
                each.Quantity += change;
                Save(each);
            }
        }

        private void ProcessTo(string stockId, DateTime toUtc, int change)
        {
            if (HasAtAsOfUtc(stockId, toUtc))
                return;

            var quantityBeforeToUtc = GetQuantityBefore(stockId, toUtc);

            InsertNew(stockId, toUtc, quantityBeforeToUtc);
        }

        private bool HasAtAsOfUtc(string stockId, DateTime asOfUtc)
        {
            return null != Query.Where(p => p.StockId == stockId).And(p => p.AsOfUtc == asOfUtc).SingleOrDefault();
        }

        private void ProcessFrom(string stockId, DateTime fromUtc, int change)
        {
            if (UpdateAtAsOfUtc(stockId, fromUtc, change))
                return;

            var quantityBeforeFromUtc = GetQuantityBefore(stockId, fromUtc);

            InsertNew(stockId, fromUtc, quantityBeforeFromUtc + change);
        }

        private bool UpdateAtAsOfUtc(string stockId, DateTime asOfUtc, int change)
        {
            var record = Query.Where(p => p.StockId == stockId).And(p => p.AsOfUtc == asOfUtc).SingleOrDefault();
            if (record == null)
                return false;

            record.Quantity += change;
            Save(record);
            return true;
        }

        private int GetQuantityBefore(string stockId, DateTime fromUtc)
        {
            var result = Query.Where(p => p.StockId == stockId).And(p => p.AsOfUtc < fromUtc)
                .OrderBy(p => p.AsOfUtc).Desc.Take(1).SingleOrDefault();

            if (result == null)
                return 0;
            return result.Quantity;
        }

        private void InsertNew(string stockId, DateTime asOfUtc, int change)
        {
            var record = new QuantityAvailableData(stockId, asOfUtc);
            record.Quantity = change;

            Save(record);
        }
    }
}