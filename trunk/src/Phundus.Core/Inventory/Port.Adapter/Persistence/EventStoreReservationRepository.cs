namespace Phundus.Core.Inventory.Port.Adapter.Persistence
{
    using Common.Events;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class EventStoreReservationRepository : EventStoreRepositoryBase, IReservationRepository
    {
        // TODO: Remove and use Get<Reservation>(ReservationId)
        public Reservation Get(ReservationId reservationId)
        {
            return Get(new EventStreamId(reservationId.Id, 1), es => new Reservation(es.Events, es.Version));
        }

        public void Save(Reservation reservation)
        {
            Append(reservation.ReservationId.Id, reservation);
        }
    }
}