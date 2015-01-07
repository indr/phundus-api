namespace Phundus.Core.Tests.Inventory.Application
{
    using System;
    using Common.Domain.Model;
    using Core.Inventory.Application.Commands;
    using Machine.Fakes;
    using Machine.Specifications;

    [Subject(typeof(ChangeReservationPeriodHandler))]
    public class when_change_reservation_period_is_handled : reservation_handler_concern<ChangeReservationPeriod, ChangeReservationPeriodHandler>
    {
        public static Period NewPeriod = new Period(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3));
        public static int MutatingEventsCount = 0;

        private Establish ctx = () =>
        {
            MutatingEventsCount = Reservation.MutatingEvents.Count;
            command = new ChangeReservationPeriod(OrganizationId, ReservationId, NewPeriod);
        };

        [Ignore("TODO")]
        public It should_ask_for_chief_or_owner_privileges = () => { };

        public It should_generate_mutating_events =
            () => Reservation.MutatingEvents.Count.ShouldBeGreaterThan(MutatingEventsCount);

        public It should_save_to_repository = () => Repository.WasToldTo(x => x.Save(Reservation));
    }
}