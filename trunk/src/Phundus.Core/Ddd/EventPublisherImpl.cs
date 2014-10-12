namespace Phundus.Core.Ddd
{
    public class EventPublisherImpl : IEventPublisher
    {
        public IEventHandlerFactory Factory { get; set; }

        public IEventStore EventStore { get; set; }

        public void Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : DomainEvent
        {
            ISubscribeTo<TDomainEvent>[] subscribers = Factory.GetSubscribersForEvent(@event);

            foreach (var each in subscribers)
                each.Handle(@event);

            EventStore.Append(@event);
        }
    }

    public interface IEventPublisher
    {
        void Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : DomainEvent;
    }
}