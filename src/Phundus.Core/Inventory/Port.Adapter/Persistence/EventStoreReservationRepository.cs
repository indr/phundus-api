namespace Phundus.Core.Inventory.Port.Adapter.Persistence
{
    using System;
    using Common.Events;
    using Domain.Model.Catalog;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class EventStoreRepositoryBase
    {
        public IEventStore EventStore { get; set; }
    }

    public class EventStoreReservationRepository : EventStoreRepositoryBase, IReservationRepository
    {
        

        public Reservation Get(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId)
        {
            // snapshots not currently supported; always use version 1

            var eventStreamId = new EventStreamId(reservationId.Id);
            var eventStream = EventStore.GetEventStreamSince(eventStreamId);

            if (eventStream.Version == 0)
                return null;

            return new Reservation(eventStream.Events, eventStream.Version);
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