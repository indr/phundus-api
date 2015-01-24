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
            ProcessTo(e, e.ToUtc, e.Change);

            ProcessFrom(e, e.FromUtc, e.Change);

            ProcessInPeriod(e, e.FromUtc, e.ToUtc, e.Change);

            RemoveRedundants(e, e.FromUtc, e.ToUtc);
        }

        private void ProcessTo(QuantityAvailableChanged e, DateTime toUtc, int change)
        {
            if (toUtc.Date == DateTime.MaxValue.Date)
                return;

            if (HasAtAsOfUtc(e.StockId, toUtc))
                return;

            var quantityBeforeToUtc = GetQuantityBefore(e.StockId, toUtc);

            InsertNew(e, toUtc, quantityBeforeToUtc);
        }

        private void ProcessFrom(QuantityAvailableChanged e, DateTime fromUtc, int change)
        {
            if (UpdateAtAsOfUtc(e.StockId, fromUtc, change))
                return;

            var quantityBeforeFromUtc = GetQuantityBefore(e.StockId, fromUtc);

            InsertNew(e, fromUtc, quantityBeforeFromUtc + change);
        }

        private void ProcessInPeriod(QuantityAvailableChanged e, DateTime fromUtc, DateTime toUtc, int change)
        {
            var records = Query.Where(p => p.StockId == e.StockId)
                .And(p => p.AsOfUtc > fromUtc).And(p => p.AsOfUtc < toUtc).List();
            foreach (var each in records)
            {
                each.Quantity += change;
                Save(each);
            }
        }

        private void RemoveRedundants(QuantityAvailableChanged e, DateTime fromUtc, DateTime toUtc)
        {
            var lastQuantity = GetQuantityBefore(e.StockId, fromUtc);

            var records = Query.Where(p => p.StockId == e.StockId)
                .And(p => p.AsOfUtc >= fromUtc).And(p => p.AsOfUtc <= toUtc).OrderBy(p => p.AsOfUtc).Asc.List();
            foreach (var each in records)
            {
                if (each.Quantity == lastQuantity)
                {
                    Delete(each);
                    continue;
                }

                lastQuantity = each.Quantity;
            }
        }

        private bool HasAtAsOfUtc(string stockId, DateTime asOfUtc)
        {
            return null != Query.Where(p => p.StockId == stockId).And(p => p.AsOfUtc == asOfUtc).SingleOrDefault();
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

        private void InsertNew(QuantityAvailableChanged e, DateTime asOfUtc, int change)
        {
            var record = new QuantityAvailableData(e.OrganizationId, e.ArticleId, e.StockId, asOfUtc);
            record.Quantity = change;

            Save(record);
        }
    }
}