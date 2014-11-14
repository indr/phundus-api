namespace Phundus.Core.Inventory.Port.Adapter.Persistence
{
    using System;
    using Common.Domain.Model;
    using Common.Events;

    public class EventStoreRepositoryBase
    {
        public IEventStore EventStore { get; set; }

        public T Get<T>(EventStreamId eventStreamId, Func<EventStream, T> factory)
        {
            // snapshots not currently supported; always use version 1

            var eventStream = EventStore.GetEventStreamSince(eventStreamId);

            if (eventStream.Version == 0)
                return default(T);

            return factory(eventStream);
        }

        protected void Append(string id, EventSourcedRootEntity aggregate)
        {
            var eventStreamId = new EventStreamId(id, aggregate.MutatedVersion);
            EventStore.Append(eventStreamId, aggregate.MutatingEvents);
        }
    }
}