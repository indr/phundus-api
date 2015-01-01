namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using Common;
    using Common.Domain.Model;
    using Common.Port.Adapter.Persistence;
    using Ddd;

    public class ReservationSagaManager : ISubscribeTo<OrderItemAdded>
    {
        private readonly ISagaRepository _sagaRepository;

        public ReservationSagaManager(ISagaRepository sagaRepository)
        {
            AssertionConcern.AssertArgumentNotNull(sagaRepository, "Saga repository must be provided.");

            _sagaRepository = sagaRepository;
        }

        public void Handle(OrderItemAdded e)
        {
            var saga =_sagaRepository.FindById<ReservationSaga>(e.OrderItemId);

            saga.Transition(e);

            _sagaRepository.Save(saga);
        }

        public void Handle(OrderItemRemoved e)
        {
            var saga = _sagaRepository.FindById<ReservationSaga>(e.OrderItemId);

            saga.Transition(e);

            _sagaRepository.Save(saga);
        }
    }
}