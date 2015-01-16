namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using Common.Domain.Model;
    using Common.EventPublishing;
    using Cqrs;
    using Ddd;
    using Orders.Model;

    public class ReservationSagaManager : SagaManager<ReservationSaga>, ISubscribeTo<OrderItemAdded>,
        ISubscribeTo<OrderItemRemoved>, ISubscribeTo<OrderItemPeriodChanged>, ISubscribeTo<OrderItemQuantityChanged>,
        ISubscribeTo<OrderRejected>, ISubscribeTo<OrderClosed>
    {
        public ReservationSagaManager(ISagaRepository repository, ICommandDispatcher dispatcher)
            : base(repository, dispatcher)
        {
        }

        public void Handle(OrderItemAdded e)
        {
            Transition(e.OrderItemId, e);
        }

        public void Handle(OrderItemPeriodChanged e)
        {
            Transition(e.OrderItemId, e);
        }

        public void Handle(OrderItemQuantityChanged e)
        {
            Transition(e.OrderItemId, e);
        }

        public void Handle(OrderItemRemoved e)
        {
            Transition(e.OrderItemId, e);
        }

        public void Handle(OrderRejected e)
        {
            foreach (var each in e.OrderItemIds)
                Transition(each, e);
        }

        public void Handle(OrderClosed e)
        {
            foreach (var each in e.OrderItemIds)
                Transition(each, e);
        }
    }
}