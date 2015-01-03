namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Domain.Model.Reservations;

    public class ChangeReservationPeriod : ICommand
    {
        public ChangeReservationPeriod(ReservationId reservationId)
        {
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");

            ReservationId = reservationId;
        }

        public ReservationId ReservationId { get; private set; }
    }
}