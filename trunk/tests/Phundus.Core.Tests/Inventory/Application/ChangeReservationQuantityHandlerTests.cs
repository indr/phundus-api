namespace Phundus.Core.Tests.Inventory.Application
{
    using System;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Reservations;
    using Core.Shop.Domain.Model.Ordering;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    [Subject(typeof (ChangeReservationQuantityHandler))]
    public class when_change_quantity_in_inventory_is_handled :
        handler_concern<ChangeReservationQuantity, ChangeReservationQuantityHandler>
    {
        public static OrganizationId OrganizationId = new OrganizationId(1001);
        public static ReservationId ReservationId = new ReservationId();
        public static ArticleId ArticleId = new ArticleId(10001);
        public static Period OldPeriod = new Period(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        public static int OldQuantity = 1;
        public static int NewQuantity = 2;

        public static Reservation Reservation = new Reservation(ReservationId, OrganizationId, ArticleId,
            new OrderId(1), OldPeriod, OldQuantity);

        public static IReservationRepository Repository;

        private Establish ctx = () =>
        {
            Repository = depends.on<IReservationRepository>();
            Repository.Expect(x => x.Get(OrganizationId, ArticleId, ReservationId)).Return(Reservation);

            MutatingEventsCount = Reservation.MutatingEvents.Count;
            command = new ChangeReservationQuantity(OrganizationId, ArticleId, ReservationId, NewQuantity);
        };

        public static int MutatingEventsCount { get; set; }

        [Ignore("TODO")]
        public It should_ask_for_chief_or_owner_privileges = () => { };

        public It should_generate_mutating_events = () => Reservation.MutatingEvents.Count.ShouldBeGreaterThan(MutatingEventsCount);

        public It should_save_to_repository = () => Repository.WasToldTo(x => x.Save(Reservation));
    }
}