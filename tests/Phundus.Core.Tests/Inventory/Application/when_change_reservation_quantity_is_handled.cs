namespace Phundus.Core.Tests.Inventory.Application
{
    using Core.Inventory.Application.Commands;
    using Machine.Fakes;
    using Machine.Specifications;

    [Subject(typeof (ChangeReservationQuantityHandler))]
    public class when_change_reservation_quantity_is_handled :
        reservation_handler_concern<ChangeReservationQuantity, ChangeReservationQuantityHandler>
    {
        protected static int NewQuantity = 2;
        protected static int MutatingEventsCount = 0;

        private Establish ctx = () =>
        {
            MutatingEventsCount = Reservation.MutatingEvents.Count;
            command = new ChangeReservationQuantity(OrganizationId, ReservationId, NewQuantity);
        };

        [Ignore("TODO")]
        public It should_ask_for_chief_or_owner_privileges = () => { };

        public It should_generate_mutating_events =
            () => Reservation.MutatingEvents.Count.ShouldBeGreaterThan(MutatingEventsCount);

        public It should_save_to_repository = () => Repository.WasToldTo(x => x.Save(Reservation));
    }
}