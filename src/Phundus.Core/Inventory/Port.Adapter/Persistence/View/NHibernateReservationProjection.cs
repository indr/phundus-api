namespace Phundus.Core.Inventory.Port.Adapter.Persistence.View
{
    using Application.Data;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence;
    using Domain.Model.Reservations;

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

        private void Process(ArticleReserved domainEvent)
        {
            var data = new ReservationData();
            data.ArticleId = domainEvent.ArticleId;
            data.ReservationId = domainEvent.ReservationId;
            data.OrganizationId = domainEvent.OrganizationId;
            data.CreatedUtc = domainEvent.OccuredOnUtc;
            data.UpdatedUtc = domainEvent.OccuredOnUtc;
            data.FromUtc = domainEvent.FromUtc;
            data.ToUtc = domainEvent.ToUtc;
            data.Quantity = domainEvent.Quantity;
            data.OrderId = domainEvent.OrderId;
            data.ReservationStatus = domainEvent.ReservationStatus;
            Save(data);
        }

        private void Process(ReservationPeriodChanged e)
        {
            var data = Find(e.ReservationId);
            data.FromUtc = e.NewFromUtc;
            data.ToUtc = e.NewToUtc;
            data.UpdatedUtc = e.OccuredOnUtc;
            Save(data);
        }

        private void Process(ReservationQuantityChanged domainEvent)
        {
            var data = Find(domainEvent.ReservationId);
            data.Quantity = domainEvent.NewQuantity;
            data.UpdatedUtc = domainEvent.OccuredOnUtc;
            Save(data);
        }
    }
}