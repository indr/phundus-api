namespace Phundus.Core.Tests.Inventory.Domain.Model.Reservations
{
    using System;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Reservations;
    using Core.Shop.Domain.Model.Ordering;
    using Machine.Specifications;

    public class reservation_concern : aggregate_root_concern<Reservation>
    {
        protected static OrganizationId organizationId;
        protected static ArticleId articleId;
        protected static ReservationId reservationId;
        protected static OrderId orderId;

        public Establish ctx = () =>
        {
            organizationId = new OrganizationId(101);
            articleId = new ArticleId(201);
            reservationId = new ReservationId();
            orderId = new OrderId(301);           
        };
    }

    [Subject(typeof (Reservation))]
    public class when_a_reservation_is_created : reservation_concern
    {
        private static Period period = new Period(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
        private static int quantity = 1;

        public Because of =
            () => { _sut = new Reservation(reservationId, organizationId, articleId, orderId, period, quantity); };

        public It should_have_mutating_event_reservation_created =
            () => _sut.MutatingEvents[0].ShouldBeOfExactType<ArticleReserved>();

        public It should_have_reservation_id = () => _sut.ReservationId.ShouldEqual(reservationId);

        public It should_have_organization_id = () => _sut.OrganizationId.ShouldEqual(organizationId);

        public It should_have_article_id = () => _sut.ArticleId.ShouldEqual(articleId);

        public It should_have_period = () => _sut.Period.ShouldEqual(period);

        public It should_have_quantity = () => _sut.Quantity.ShouldEqual(quantity);

        public It should_have_status_new = () => _sut.Status.ShouldEqual(ReservationStatus.New);
    }

    [Subject(typeof (Reservation))]
    public class when_reservation_period_is_changed : reservation_concern
    {
        private static Period oldPeriod = new Period(DateTime.Today, DateTime.Today.AddDays(1));
        private static Period newPeriod = new Period(DateTime.Today.AddDays(2), DateTime.Today.AddDays(3));

        public Establish ctx =
            () => { _sut = new Reservation(reservationId, organizationId, articleId, orderId, oldPeriod, 1); };

        public Because of = () => _sut.ChangePeriod(newPeriod);

        public It should_have_mutating_event_reservation_period_changed =
            () => _sut.MutatingEvents[1].ShouldBeOfExactType<ReservationPeriodChanged>();

        public It should_have_updated_period =
            () => _sut.Period.ShouldEqual(newPeriod);
    }

    [Subject(typeof (Reservation))]
    public class when_reservation_quantity_is_changed : reservation_concern
    {
        private static int oldQuantity = 1;
        private static int newQuantity = 2;

        public Establish ctx = () =>
        {
            _sut = new Reservation(reservationId, organizationId, articleId, orderId,
                new Period(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), oldQuantity);
        };

        public Because of = () => _sut.ChangeQuantity(newQuantity);

        public It should_have_mutating_event_reservation_quantity_changed =
            () => _sut.MutatingEvents[1].ShouldBeOfExactType<ReservationQuantityChanged>();

        public It should_have_updated_quantity =
            () => _sut.Quantity.ShouldEqual(newQuantity);
    }

    [Subject(typeof (Reservation))]
    public class when_a_reservation_is_cancelled : reservation_concern
    {
        public Establish ctx = () =>
        {
            _sut = new Reservation(reservationId, organizationId, articleId, orderId,
                new Period(DateTime.UtcNow, DateTime.UtcNow.AddDays(1)), 1);
        };

        public Because of = () => _sut.Cancel();

        public It should_have_mutating_event_reservation_cancelled =
            () => _sut.MutatingEvents[1].ShouldBeOfExactType<ReservationCancelled>();

        public It should_have_status_cancelled =
            () => _sut.Status.ShouldEqual(ReservationStatus.Cancelled);
    }

    public class when_a_cancelled_reservation : reservation_concern
    {
        protected static Exception Exception;

        private Establish ctx = () =>
        {
            _sut = new Reservation(reservationId, organizationId, articleId, orderId,
                new Period(DateTime.Today, DateTime.Today.AddDays(1)), 1);
            _sut.Cancel();
        };

        
    }

    [Subject(typeof(Reservation))]
    public class when_cancelled_reservation_quantity_is_changed : when_a_cancelled_reservation
    {
        public Because of = () => Exception = Catch.Exception(() => _sut.ChangeQuantity(2));

        public It should_fail = () => Exception.ShouldBeOfExactType<InvalidOperationException>();

        public It should_fail_with_message =
            () => Exception.Message.ShouldEqual("A cancelled reservation can not be modified.");
    }

    [Subject(typeof(Reservation))]
    public class when_cancelled_reservation_period_is_changed : when_a_cancelled_reservation
    {
        public Because of = () => Exception = Catch.Exception(() => _sut.ChangePeriod(new Period(DateTime.Today.AddDays(2), DateTime.Today.AddDays(3))));

        public It should_fail = () => Exception.ShouldBeOfExactType<InvalidOperationException>();

        public It should_fail_with_message =
            () => Exception.Message.ShouldEqual("A cancelled reservation can not be modified.");
    }
}