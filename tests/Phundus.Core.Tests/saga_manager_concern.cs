namespace Phundus.Core.Tests
{
    using System;
    using Common.Domain.Model;
    using Core.Cqrs;
    using Machine.Fakes;
    using Machine.Specifications;

    public class saga_manager_concern<TSaga, TSagaManager> : concern<TSagaManager> where TSagaManager : class
        where TSaga : class, ISaga, new()
    {
        protected static IDomainEvent _event;
        protected static ISagaRepository _sagaRepository;
        protected static TSaga _saga;
        protected static Guid _correlationId;

        private Establish ctx = () =>
        {
            _saga = mock.stub<TSaga>();
            depends.on<ICommandDispatcher>();
            _sagaRepository = depends.on<ISagaRepository>();
        };

        private Because of = () => _sagaRepository.WhenToldTo(x => x.GetById<TSaga>(_correlationId)).Return(_saga);

        protected static void AssertTransition()
        {
            AssertTransition(_saga);
        }

        protected static void AssertTransition(TSaga saga)
        {
            saga.WasToldTo(x => x.Transition(_event));
        }

        protected static void AssertRepositoryGetById(string correlationId)
        {
            AssertRepositoryGetById(new Guid(correlationId));
        }

        protected static void AssertRepositoryGetById(Guid correlationId)
        {
            _sagaRepository.WasToldTo(x => x.GetById<TSaga>(correlationId));
        }
    }
}