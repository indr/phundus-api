namespace Phundus.Core.Tests.Inventory.Domain.Model.Reservations
{
    using System;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Inventory.Domain.Model.Reservations;
    using Core.Shop.Domain.Model.Ordering;
    using Machine.Specifications;

    [Subject(typeof (AllocationSagaManager))]
    public class allocation_saga_manager_concern : saga_manager_concern<AllocationSaga, AllocationSagaManager>
    {
        protected static OrganizationId _organizationId = new OrganizationId(1001);
        protected static ArticleId _articleId = new ArticleId(10001);
        protected static ReservationId _reservationId = new ReservationId();
        protected static OrderId _orderId = new OrderId(1);
        protected static Period _period = Period.FromTodayToTomorrow;
        protected static int _quantity = 1;

        private Establish ctx = () => _correlationId = new Guid(_reservationId.Id);

        private Because of = () => sut.Handle((dynamic) _event);
    }

    public class when_article_reserved_is_handled : allocation_saga_manager_concern
    {
        private Establish ctx =
            () => _event = new ArticleReserved(_organizationId, _articleId, _reservationId, _orderId, _period, _quantity, ReservationStatus.New);

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_reservationId.Id);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }

    public class when_reservation_cancelled_is_handled : allocation_saga_manager_concern
    {
        private Establish ctx =
            () => _event = new ReservationCancelled(_organizationId, _articleId, _reservationId, _period, _quantity);

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_reservationId.Id);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }

    public class when_reservation_period_changed_is_handled : allocation_saga_manager_concern
    {
        private Establish ctx =
            () => _event = new ReservationPeriodChanged(_organizationId, _articleId, _reservationId, _period, _period);

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_reservationId.Id);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }

    public class when_reservation_quantity_changed_is_handled : allocation_saga_manager_concern
    {
        private Establish ctx =
            () =>
                _event =
                    new ReservationQuantityChanged(_organizationId, _articleId, _reservationId, _quantity, _quantity);

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_reservationId.Id);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }
}