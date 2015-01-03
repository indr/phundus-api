namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Cqrs;
    using Domain.Model.Reservations;

    public class ChangeReservationQuantity : ICommand
    {
        public ChangeReservationQuantity(ReservationId reservationId)
        {
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");

            ReservationId = reservationId;
        }

        public ReservationId ReservationId { get; private set; }
    }

    public class ChangeReservationQuantityHandler : IHandleCommand<ChangeReservationQuantity>
    {
        public void Handle(ChangeReservationQuantity command)
        {
            throw new System.NotImplementedException("ChangeReservatoinQuantityHandler.Handle()");
        }
    }
}