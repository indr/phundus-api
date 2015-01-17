namespace Phundus.Core.Ddd
{
    using Common;
    using Common.Domain.Model;
    using Common.EventPublishing;
    using Common.Events;

    /// <summary>
    /// Former publisher to notify ISubscribeTo-Handlers. ISubscribeTo-Handlers are now notified through notification producer/consumer.
    /// </summary>
    public class EventPublisherImpl : IEventPublisher
    {
        private readonly IEventStore _eventStore;

        public EventPublisherImpl(IEventStore eventStore)
        {
            AssertionConcern.AssertArgumentNotNull(eventStore, "Event store must be provided.");

            _eventStore = eventStore;
        }

        public void Publish<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : DomainEvent
        {
            _eventStore.Append(domainEvent);
        }
    }
}