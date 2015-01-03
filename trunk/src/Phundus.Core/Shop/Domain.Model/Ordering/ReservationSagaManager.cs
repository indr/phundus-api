namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;

    public class ReservationSagaManager : SagaManager<ReservationSaga>, ISubscribeTo<OrderItemAdded>,
        ISubscribeTo<OrderItemRemoved>, ISubscribeTo<OrderItemPeriodChanged>, ISubscribeTo<OrderItemQuantityChanged>
    {
        public ReservationSagaManager(ISagaRepository repository, ICommandDispatcher dispatcher) : base(repository, dispatcher)
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
    }
}