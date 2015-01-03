namespace Phundus.Core.Ddd
{
    using System;
    using Common.Domain.Model;
    using Cqrs;

    public class SagaManager<TSaga> where TSaga : ISaga, new()
    {
        private ICommandDispatcher _dispatcher;
        private ISagaRepository _repository;

        public SagaManager(ISagaRepository repository, ICommandDispatcher dispatcher)
        {
            _repository = repository;
            _dispatcher = dispatcher;
        }

        protected void Transition(Guid sagaId, IDomainEvent e)
        {
            var saga = _repository.GetById<TSaga>(sagaId);

            saga.Transition(e);

            _repository.Save(saga);

            foreach (var each in saga.UndispatchedCommands)
                _dispatcher.Dispatch((dynamic) each);
        }
    }
}