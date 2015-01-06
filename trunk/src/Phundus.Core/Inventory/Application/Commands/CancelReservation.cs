namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class CancelReservation : ICommand
    {
        public CancelReservation(OrganizationId organizationId, ReservationId reservationId)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided");
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");

            OrganizationId = organizationId;
            ReservationId = reservationId;
        }

        public OrganizationId OrganizationId { get; private set; }
        public ReservationId ReservationId { get; private set; }
    }
}