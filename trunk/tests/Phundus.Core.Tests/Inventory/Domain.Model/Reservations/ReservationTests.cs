namespace Phundus.Core.Tests.Inventory.Domain.Model.Reservations
{
    using System;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Reservations;
    using Machine.Specifications;

    public class reservation_convern : aggregate_root_concern<Reservation>
    {
        protected static OrganizationId organizationId;
        protected static ArticleId articleId;
        protected static ReservationId reservationId;

        public Establish ctx = () =>
        {
            organizationId = new OrganizationId(101);
            articleId = new ArticleId(201);
            reservationId = new ReservationId("0123456789");
        };
    }

    [Subject(typeof (Reservation))]
    public class when_a_reservation_is_created : reservation_convern
    {
        public Because of =
            () =>
            {
                sut = new Reservation(organizationId, articleId, reservationId,
                    new TimeRange(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), 1);
            };

        public It should_have_mutating_event_reservation_created =
            () => sut.MutatingEvents[0].ShouldBeOfExactType<ReservationCreated>();
    }

    [Subject(typeof (Reservation))]
    public class when_time_range_is_changed : reservation_convern
    {
        public Establish ctx = () =>
        {
            sut = new Reservation(organizationId, articleId, reservationId,
                new TimeRange(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), 1);
        };

        public Because of = () => sut.ChangeTimeRange(new TimeRange(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));

        public It should_have_mutating_event_reservation_time_range_changed =
            () => sut.MutatingEvents[1].ShouldBeOfExactType<ReservationTimeRangeChanged>();
    }

    public class when_amount_is_changed : reservation_convern
    {
        public Establish ctx = () =>
        {
            sut = new Reservation(organizationId, articleId, reservationId,
                new TimeRange(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), 1);
        };

        public Because of = () => sut.ChangeAmount(2);

        public It should_have_mutating_event_reservation_time_range_changed =
            () => sut.MutatingEvents[1].ShouldBeOfExactType<ReservationAmountChanged>();
    }
}