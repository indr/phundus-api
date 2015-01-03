namespace Phundus.Core.Ddd
{
    using System;
    using Common.Domain.Model;
    using Cqrs;

    public class SagaManager<TSaga> where TSaga : ISaga, new()
    {
        public ISagaRepository Repository { get; set; }

        public ICommandDispatcher CommandDispatcher { get; set; }

        protected void Transition(Guid sagaId, IDomainEvent e)
        {
            var saga = Repository.GetById<TSaga>(sagaId);

            saga.Transition(e);

            Repository.Save(saga);

            foreach (var each in saga.UndispatchedCommands)
                CommandDispatcher.Dispatch((dynamic) each);
        }
    }
}