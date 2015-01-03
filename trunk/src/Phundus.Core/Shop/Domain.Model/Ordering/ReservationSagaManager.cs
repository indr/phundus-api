namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using Ddd;

    public class ReservationSagaManager : SagaManager<ReservationSaga>, ISubscribeTo<OrderItemAdded>,
        ISubscribeTo<OrderItemRemoved>, ISubscribeTo<OrderItemPeriodChanged>, ISubscribeTo<OrderItemQuantityChanged>
    {
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