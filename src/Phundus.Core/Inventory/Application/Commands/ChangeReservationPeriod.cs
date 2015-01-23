namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Common.Domain.Model;
    using Cqrs;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class ChangeReservationPeriod : ICommand
    {
        public ChangeReservationPeriod(OrganizationId organizationId, ReservationId reservationId,
            Period period)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");

            OrganizationId = organizationId;
            ReservationId = reservationId;
            Period = period;
        }

        public OrganizationId OrganizationId { get; private set; }
        public ReservationId ReservationId { get; private set; }
        public Period Period { get; private set; }
    }

    public class ChangeReservationPeriodHandler : IHandleCommand<ChangeReservationPeriod>
    {
        public IReservationRepository Repository { get; set; }

        public void Handle(ChangeReservationPeriod command)
        {
            var reservation = Repository.Get(command.ReservationId);

            reservation.ChangePeriod(command.Period);

            Repository.Save(reservation);
        }
    }
}