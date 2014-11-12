namespace Phundus.Core.Inventory.Port.Adapter.Persistence.View
{
    using System;
    using Application.Data;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence;
    using Domain.Model.Management;

    public class NHibernateQuantityInInventoryProjection : NHibernateProjectionBase<QuantityInInventoryData>,
        IDomainEventHandler
    {
        public void Handle(IDomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        private void Process(DomainEvent domainEvent)
        {
            // Fallback
        }

        private void Process(QuantityInInventoryIncreased e)
        {
            var record = Query.Where(p => p.AsOfUtc == e.AsOfUtc).SingleOrDefault();
            if (record == null)
                record = new QuantityInInventoryData();

            record.AsOfUtc = e.AsOfUtc;
            record.Total = e.Total;
            record.Change = e.Change;
            Save(record);

            UpdateFutures(e.AsOfUtc, e.Change);
        }

        private void Process(QuantityInInventoryDecreased e)
        {
            var record = Query.Where(p => p.AsOfUtc == e.AsOfUtc).SingleOrDefault();
            if (record == null)
                record = new QuantityInInventoryData();

            record.AsOfUtc = e.AsOfUtc;
            record.Total = e.Total;
            record.Change = e.Change * -1;
            Save(record);

            UpdateFutures(e.AsOfUtc, e.Change * -1);
        }

        private void UpdateFutures(DateTime asOfUtc, int change)
        {
            var records = Session.QueryOver<QuantityInInventoryData>().Where(p => p.AsOfUtc > asOfUtc).List();
            foreach (var each in records)
            {
                each.Total += change;
                Save(each);
            }
        }
    }
}