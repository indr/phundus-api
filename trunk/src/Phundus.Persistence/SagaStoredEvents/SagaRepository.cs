namespace Phundus.Persistence.SagaStoredEvents
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Common.Events;

    public class SagaRepository : ISagaRepository
    {
        private readonly ISagaEventStore _sagaEventStore;

        public SagaRepository(ISagaEventStore sagaEventStore)
        {
            AssertionConcern.AssertArgumentNotNull(sagaEventStore, "Saga event store must be provided.");

            _sagaEventStore = sagaEventStore;
        }

        public TSaga GetById<TSaga>(Guid sagaId) where TSaga : ISaga, new()
        {
            return BuildSaga<TSaga>(sagaId, GetEventStream(sagaId));
        }

        public void Save(ISaga saga)
        {
            _sagaEventStore.Append(new EventStreamId(saga.Id.ToString(), saga.MutatedVersion), saga.UncommittedEvents);
        }

        private TSaga BuildSaga<TSaga>(Guid sagaId, EventStream eventStream) where TSaga : ISaga, new()
        {
            // TODO: Set saga id not via public setter
            var saga = new TSaga();
            saga.Id = sagaId;
            saga.MutatedVersion = eventStream.Version;
            foreach (var domainEvent in eventStream.Events)
                saga.Transition(domainEvent);

            saga.ClearUncommittedEvents();
            saga.ClearUndispatchedCommands();

            return saga;
        }

        private EventStream GetEventStream(Guid sagaId)
        {
            return _sagaEventStore.GetEventStreamSince(new EventStreamId(sagaId.ToString()));
        }
    }
}