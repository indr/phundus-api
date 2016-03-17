namespace Phundus.Common.Infrastructure.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Common;
    using Common.Domain.Model;
    using Common.Eventing;

    public class EventSourcedRepositoryBase
    {
    }

    public class EventSourcedRepositoryBase<TAggregate> : EventSourcedRepositoryBase
    {
        public IEventStore EventStore { get; set; }

        protected TAggregate GetById(GuidIdentity aggregateId)
        {
            var eventStream = EventStore.LoadEventStream(aggregateId);

            if (eventStream.Events.Count == 0)
                throw new AggregateNotFoundException(typeof (TAggregate).Name, aggregateId);

            var constructor = GetConstructor();

            return (TAggregate) constructor.Invoke(new object[] {eventStream.Events, eventStream.Version});
        }


        private ConstructorInfo GetConstructor()
        {
            var result = typeof (TAggregate).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null,
                new[] {typeof (IEnumerable<IDomainEvent>), typeof (int)}, null);

            if (result == null)
                throw new ArgumentException("Could not find constructor of aggregate type " + typeof (TAggregate).Name);

            return result;
        }
    }
}