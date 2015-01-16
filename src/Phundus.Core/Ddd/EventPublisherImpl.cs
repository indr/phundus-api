namespace Phundus.Core.Ddd
{
    using Common.Domain.Model;
    using Common.EventPublishing;
    using Common.Events;

    public class EventPublisherImpl : IEventPublisher
    {
        public IEventHandlerFactory Factory { get; set; }

        public IEventStore EventStore { get; set; }

        public void Publish<TDomainEvent>(TDomainEvent @event, bool store = true) where TDomainEvent : DomainEvent
        {
            ISubscribeTo<TDomainEvent>[] subscribers = Factory.GetSubscribersForEvent(@event);

            foreach (var each in subscribers)
                each.Handle(@event);

            if (store)
                EventStore.Append(@event);
        }
    }
}