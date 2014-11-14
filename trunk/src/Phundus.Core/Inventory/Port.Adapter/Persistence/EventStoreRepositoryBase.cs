namespace Phundus.Core.Inventory.Port.Adapter.Persistence
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Common.Events;

    public class EventStoreRepositoryBase
    {
        public IEventStore EventStore { get; set; }

        public T Get<T>(EventStreamId eventStreamId, Func<EventStream, T> entityFactory)
        {
            AssertionConcern.AssertArgumentNotNull(eventStreamId, "Event stream id must be provided.");
            AssertionConcern.AssertArgumentNotNull(entityFactory, "Entity factory method must be provided." );

            // snapshots not currently supported; always use version 1
            var eventStream = EventStore.GetEventStreamSince(eventStreamId);

            if (eventStream.Version == 0)
                return default(T);

            return entityFactory(eventStream);
        }

        protected void Append(string id, EventSourcedRootEntity aggregate)
        {
            AssertionConcern.AssertArgumentNotEmpty(id, "Id must be provided.");
            AssertionConcern.AssertArgumentNotNull(aggregate, "Aggregate must be provided.");

            var eventStreamId = new EventStreamId(id, aggregate.MutatedVersion);
            EventStore.Append(eventStreamId, aggregate.MutatingEvents);
        }
    }
}