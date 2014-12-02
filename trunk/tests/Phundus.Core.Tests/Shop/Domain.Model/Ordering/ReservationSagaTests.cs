namespace Phundus.Core.Tests.Shop.Domain.Model.Ordering
{
    using System;
    using Common.Domain.Model;
    using Core.IdentityAndAccess.Domain.Model.Organizations;
    using Core.IdentityAndAccess.Domain.Model.Users;
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders.Model;
    using Machine.Specifications;

    public class saga_concern<TSaga> : concern<TSaga> where TSaga : class, ISaga
    {
        protected static IDomainEvent domainEvent;

        public Because of = () => sut.Transition(domainEvent);
    }

    public class reservation_saga_concern : saga_concern<ReservationSaga>
    {
        protected static UserId initiatorId =  new UserId(1);
        protected static OrganizationId organizationId = new OrganizationId(2);
        protected static OrderId orderId = new OrderId(3);
        protected static Guid orderItemId = new Guid();
        protected static ArticleId articleId = new ArticleId(4);
        protected static DateTime fromUtc = DateTime.Today;
        protected static DateTime toUtc = DateTime.Today.AddDays(1);
        protected static int quantity = 1;
    }

    public class when_order_item_added_is_transitioned : reservation_saga_concern
    {
        public Establish ctx = () =>
        {
            domainEvent = new OrderItemAdded(initiatorId, organizationId, orderId, orderItemId, articleId, fromUtc, toUtc, quantity);
        };

        public It should_have_uncommitted_event = () => sut.UncommittedEvents.ShouldContainOnly(domainEvent);

        public It should_have_undispatched_command_create_reservation = () => sut.UndispatchedCommands.ShouldNotBeEmpty();
        
    }
}