namespace Phundus.Core.Tests.Shop.Domain.Model.Ordering
{
    using Common.Domain.Model;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders.Model;
    using Machine.Specifications;

    public class saga_concern<TSaga> : concern<TSaga> where TSaga : class, ISaga
    {
        protected static IDomainEvent domainEvent;

        public Because of = () => sut.Transition(domainEvent);
    }

    public class when_order_item_added_is_transitioned : saga_concern<ReservationSaga>
    {
        public Establish ctx = () =>
        {
            domainEvent = new OrderItemAdded();
        };

        public It should_have_uncommitted_event = () => sut.UncommittedEvents.ShouldContainOnly(domainEvent);

        public It should_have_undispatched_command_create_reservation = () => sut.UndispatchedCommands.ShouldNotBeEmpty();
    }
}