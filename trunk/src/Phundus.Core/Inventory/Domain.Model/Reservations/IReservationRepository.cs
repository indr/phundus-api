namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    public interface IReservationRepository
    {
        Reservation Get(ReservationId reservationId);

        void Save(Reservation reservation);
    }
}