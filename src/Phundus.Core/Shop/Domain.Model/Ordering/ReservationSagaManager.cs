namespace Phundus.Core.Shop.Domain.Model.Ordering
{
    using Common;
    using Common.Cqrs;
    using Common.Domain.Model;
    using Cqrs;
    using Ddd;

    public class ReservationSagaManager : ISubscribeTo<OrderItemAdded>
    {
        private readonly ISagaRepository _repository;

        public ICommandDispatcher CommandDispatcher { get; set; }

        public ReservationSagaManager(ISagaRepository repository)
        {
            AssertionConcern.AssertArgumentNotNull(repository, "Saga repository must be provided.");

            _repository = repository;
        }

        public void Handle(OrderItemAdded e)
        {
            var saga = _repository.GetById<ReservationSaga>(e.OrderItemId);

            saga.Transition(e);

            _repository.Save(saga);
            foreach (var each in saga.UndispatchedCommands)
                CommandDispatcher.Dispatch((dynamic)each);
        }

        public void Handle(OrderItemRemoved e)
        {

            var saga = _repository.GetById<ReservationSaga>(e.OrderItemId);

            saga.Transition(e);

            _repository.Save(saga);
            foreach (var each in saga.UndispatchedCommands)
                CommandDispatcher.Dispatch(each);
        }
    }
}