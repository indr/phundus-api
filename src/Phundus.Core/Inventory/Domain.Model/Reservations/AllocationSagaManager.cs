namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using Common.Domain.Model;
    using Common.EventPublishing;
    using Cqrs;
    using Ddd;

    public class AllocationSagaManager : SagaManager<AllocationSaga>, ISubscribeTo<ArticleReserved>,
        ISubscribeTo<ReservationCancelled>, ISubscribeTo<ReservationPeriodChanged>,
        ISubscribeTo<ReservationQuantityChanged>
    {
        public AllocationSagaManager(ISagaRepository repository, ICommandDispatcher dispatcher)
            : base(repository, dispatcher)
        {
        }

        public void Handle(ArticleReserved @event)
        {
            Transition(@event.ReservationId, @event);
        }

        public void Handle(ReservationCancelled @event)
        {
            Transition(@event.ReservationId, @event);
        }

        public void Handle(ReservationPeriodChanged @event)
        {
            Transition(@event.ReservationId, @event);
        }

        public void Handle(ReservationQuantityChanged @event)
        {
            Transition(@event.ReservationId, @event);
        }
    }
}