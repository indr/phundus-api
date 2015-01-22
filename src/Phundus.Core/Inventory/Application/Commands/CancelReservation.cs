namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Cqrs;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;
    using IdentityAndAccess.Domain.Model.Users;
    using IdentityAndAccess.Queries;

    public class CancelReservation : ICommand
    {
        public CancelReservation(UserId initiatorId, OrganizationId organizationId, ReservationId reservationId)
        {
            AssertionConcern.AssertArgumentNotNull(initiatorId, "Initiator id must be provided.");
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided");
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");

            InitiatorId = initiatorId;
            OrganizationId = organizationId;
            ReservationId = reservationId;
        }

        public UserId InitiatorId { get; private set; }

        public OrganizationId OrganizationId { get; private set; }

        public ReservationId ReservationId { get; private set; }
    }

    public class CancelReservationHandler : IHandleCommand<CancelReservation>
    {
        private IMemberInRole _memberInRole;
        private IReservationRepository _reservationRepository;

        public CancelReservationHandler(IMemberInRole memberInRole, IReservationRepository reservationRepository)
        {
            _memberInRole = memberInRole;
            _reservationRepository = reservationRepository;
        }

        public void Handle(CancelReservation command)
        {
            _memberInRole.ActiveChief(command.OrganizationId, command.InitiatorId);

            var reservation = _reservationRepository.Get(command.ReservationId);

            reservation.Cancel();

            _reservationRepository.Save(reservation);
        }
    }
}