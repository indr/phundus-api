namespace Phundus.Core.Inventory.Port.Adapter.Persistence
{
    using Common.Events;
    using Domain.Model.Catalog;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class EventStoreReservationRepository : EventStoreRepositoryBase, IReservationRepository
    {
        public Reservation Get(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId)
        {
            return Get(new EventStreamId(reservationId.Id), es => new Reservation(es.Events, es.Version));
        }

        public ReservationId GetNextIdentity()
        {
            return new ReservationId();
        }

        public void Save(Reservation reservation)
        {
            Append(reservation.ReservationId.Id, reservation);
        }
    }
}