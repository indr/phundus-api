namespace Phundus.Core.Inventory.Port.Adapter.Persistence
{
    using System;
    using Common.Events;
    using Domain.Model.Articles;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class EventStoreReservationRepository : IReservationRepository
    {
        public IEventStore EventStore { get; set; }

        public Reservation Get(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId)
        {
            // snapshots not currently supported; always use version 1
            //EventStreamId eventId = new EventStreamId(aTenant.id(), aCalendarId.id());
            //EventStream eventStream = this.eventStore().eventStreamSince(eventId);
            //var reservation = new Reservation(eventStream.events(), eventStream.version());
            //return reservation;
            throw new NotImplementedException();
        }

        public ReservationId GetNextIdentity()
        {
            return new ReservationId();
        }

        public void Save(Reservation reservation)
        {
            var eventStreamId = new EventStreamId(reservation.ReservationId.Id, reservation.MutatedVersion);

            EventStore.Append(eventStreamId, reservation.MutatingEvents);
        }
    }
}