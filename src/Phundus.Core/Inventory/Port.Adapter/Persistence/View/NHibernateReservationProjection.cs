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