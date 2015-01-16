namespace Phundus.Core.Tests.Shop.Domain.Model.Ordering
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders.Model;
    using Machine.Specifications;

    public class reservation_saga_manager_concerns : saga_manager_concern<ReservationSaga, ReservationSagaManager>
    {
        protected static UserId _initiatorId = new UserId(1);
        protected static OrganizationId _organizationId = new OrganizationId(1001);
        protected static OrderId _orderId = new OrderId(10);
        protected static Guid _orderItemId = Guid.NewGuid();
        protected static ArticleId _articleId = new ArticleId(10001);
        protected static Period _period = Period.FromTodayToTomorrow;
        protected static int _quantity = 1;

        private Establish ctx = () => _correlationId = _orderItemId;

        private Because of = () => sut.Handle((dynamic)_event);
    }

    public class when_order_item_added_is_handled : reservation_saga_manager_concerns
    {
        private Establish ctx = () =>
                _event = new OrderItemAdded(_initiatorId, _organizationId, _orderId, _orderItemId, _articleId, _period, _quantity);

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_orderItemId);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }

    public class when_order_item_removed_is_handled : reservation_saga_manager_concerns
    {
        private Establish ctx = () =>
                _event = new OrderItemRemoved(_initiatorId, _organizationId, _orderId, _orderItemId, _articleId);

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_orderItemId);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }

    public class when_order_item_period_changed_is_handled : reservation_saga_manager_concerns
    {
        private Establish ctx = () =>
                _event = new OrderItemPeriodChanged(_initiatorId, _organizationId, _orderId, _orderItemId, _articleId, _period, _period);

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_orderItemId);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }

    public class when_order_item_quantity_changed_is_handled : reservation_saga_manager_concerns
    {
        private Establish ctx = () =>
                _event = new OrderItemQuantityChanged(_initiatorId, _organizationId, _orderId, _orderItemId, _articleId, _quantity, _quantity);

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_orderItemId);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }

    public class when_order_closed_is_handled : reservation_saga_manager_concerns
    {
        private Establish ctx = () =>
            _event = new OrderClosed(_initiatorId, _organizationId, _orderId, new List<Guid> {_orderItemId});

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_orderItemId);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }

    public class when_order_rejected_is_handled : reservation_saga_manager_concerns
    {
        private Establish ctx = () =>
            _event = new OrderRejected(_initiatorId, _organizationId, _orderId, new List<Guid> { _orderItemId });

        public It should_get_saga_by_reservation_id = () => AssertRepositoryGetById(_orderItemId);

        public It should_tell_saga_to_transition = () => AssertTransition();
    }
}