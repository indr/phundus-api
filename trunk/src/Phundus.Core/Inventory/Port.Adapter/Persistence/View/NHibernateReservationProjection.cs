namespace Phundus.Core.Inventory.Port.Adapter.Persistence.View
{
    using System;
    using System.Linq;
    using Application.Data;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence;
    using Domain.Model.Management;
    using Domain.Model.Reservations;
    using NHibernate.Linq;

    public class NHibernateQuantitiesInInventoryProjection : NHibernateProjectionBase<QuantitiesInInventoryData>,
        IDomainEventHandler
    {
        public void Handle(IDomainEvent domainEvent)
        {
            Process((dynamic)domainEvent);
        }
        
        private void Process(DomainEvent domainEvent)
        {
            // Fallback
        }

        private void Process(QuantityInInventoryIncreased e)
        {
            var record = Session.QueryOver<QuantitiesInInventoryData>().Where(p => p.AsOfUtc == e.AsOfUtc).SingleOrDefault();
            if (record == null)
                record = new QuantitiesInInventoryData();

            record.AsOfUtc = e.AsOfUtc;
            record.Total = e.Total;
            record.Change = e.Quantity;
            Save(record);

            var records = Session.QueryOver<QuantitiesInInventoryData>().Where(p => p.AsOfUtc > e.AsOfUtc).List();
            foreach (var each in records)
            {
                each.Total += e.Quantity;
                Save(each);
            }
        }
    }

    public class QuantitiesInInventoryData
    {
        public int Total { get; set; }
        public int Change { get; set; }
        public DateTime AsOfUtc { get; set; }
    }

    public class NHibernateReservationProjection : NHibernateProjectionBase<ReservationData>, IDomainEventHandler
    {
        public void Handle(IDomainEvent domainEvent)
        {
            Process((dynamic) domainEvent);
        }

        private void Process(DomainEvent domainEvent)
        {
            // Fallback
        }

        private void Process(ReservationAmountChanged domainEvent)
        {
            var data = Find(domainEvent.ReservationId);
            data.Amount = domainEvent.Amount;
            data.UpdatedUtc = domainEvent.OccuredOnUtc;
            Save(data);
        }

        private void Process(ReservationCreated domainEvent)
        {
            var data = new ReservationData();
            data.ArticleId = domainEvent.ArticleId;
            data.ReservationId = domainEvent.ReservationId;
            data.OrganizationId = domainEvent.OrganizationId;
            data.CreatedUtc = domainEvent.OccuredOnUtc;
            data.UpdatedUtc = domainEvent.OccuredOnUtc;
            data.FromUtc = domainEvent.FromUtc;
            data.ToUtc = domainEvent.ToUtc;
            data.Amount = domainEvent.Amount;
            Save(data);
        }
    }
}