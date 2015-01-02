namespace Phundus.Core.Tests.Shop.Domain.Model.Ordering
{
    using System;
    using System.Linq;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Application.Commands;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Domain.Model.Ordering;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;

    public class reservation_saga_concern : saga_concern<ReservationSaga>
    {
        protected static UserId initiatorId = new UserId(1);
        protected static OrganizationId organizationId = new OrganizationId(2);
        protected static OrderId orderId = new OrderId(3);
        protected static Guid orderItemId = new Guid();
        protected static ArticleId articleId = new ArticleId(4);
        protected static Period period = new Period(DateTime.Today, DateTime.Today.AddDays(1));
        protected static DateTime fromUtc = DateTime.Today;
        protected static DateTime toUtc = DateTime.Today.AddDays(1);
        protected static int quantity = 1;
    }

    public class when_order_item_added_is_transitioned : reservation_saga_concern
    {
        public Establish ctx =
            () =>
            {
                domainEvent = new OrderItemAdded(initiatorId, organizationId, orderId, orderItemId, articleId, period,
                    quantity);
            };

        public It should_have_one_undispatched_command =
            () => sut.UndispatchedCommands.Count.ShouldEqual(1);

        public It should_have_uncommitted_event = () => sut.UncommittedEvents.ShouldContainOnly(domainEvent);

        public It should_have_undispatched_command_reserve_article = () =>
        {
            var command = sut.UndispatchedCommands.First().ShouldBeAn<ReserveArticle>();
            command.OrderId.ShouldEqual(orderId);
            command.ReservationId.Id.ShouldEqual(orderItemId.ToString());
        };
    }

    public class when_order_item_removed_is_transitioned : reservation_saga_concern
    {
        public Establish ctx = () =>
        {
            pastEvents.Add(new OrderItemAdded(initiatorId, organizationId, orderId, orderItemId, articleId, period,
                quantity));
            domainEvent = new OrderItemRemoved(initiatorId, organizationId, orderId, orderItemId);
        };

        public It should_have_uncommitted_event = () => sut.UncommittedEvents.ShouldContainOnly(domainEvent);

        public It should_have_undispatched_command_cancel_reservation = () =>
        {
            var command = sut.UndispatchedCommands.First().ShouldBeAn<CancelReservation>();
            command.ReservationId.Id.ShouldEqual(orderItemId.ToString());
        };
    }

    public class when_order_item_quantity_changed_is_transitioned : reservation_saga_concern
    {
        public Establish ctx = () =>
        {
            pastEvents.Add(new OrderItemAdded(initiatorId, organizationId, orderId, orderItemId, articleId, period,
                quantity));
            domainEvent = new OrderItemQuantityChanged(initiatorId, organizationId, orderId, orderItemId, quantity + 1);
        };

        public It should_have_uncomitted_event = () => sut.UncommittedEvents.ShouldContainOnly(domainEvent);

        public It should_have_undispatched_command = () =>
        {
            var command = sut.UndispatchedCommands.First().ShouldBeAn<ChangeReservationQuantity>();
            command.ReservationId.Id.ShouldEqual(orderItemId.ToString());
        };
    }

    public class when_order_item_period_changed_is_transitioned : reservation_saga_concern
    {
        public Establish ctx = () =>
        {
            pastEvents.Add(new OrderItemAdded(initiatorId, organizationId, orderId, orderItemId, articleId, period,
                quantity));
            domainEvent = new OrderItemPeriodChanged(initiatorId, organizationId, orderId, orderItemId,
                new Period(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2)));
        };

        public It should_have_uncomitted_event = () => sut.UncommittedEvents.ShouldContainOnly(domainEvent);

        public It should_have_undispatched_command = () =>
        {
            var command = sut.UndispatchedCommands.First().ShouldBeAn<ChangeReservationPeriod>();
            command.ReservationId.Id.ShouldEqual(orderItemId.ToString());
        };
    }
}