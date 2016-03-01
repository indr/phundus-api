namespace Phundus.Common.Eventing
{
    using Domain.Model;

    public interface IEventPublisher
    {
        void Publish<TDomainEvent>(TDomainEvent e) where TDomainEvent : DomainEvent;
    }

    public class EventPublisherImpl : IEventPublisher
    {
        public IEventHandlerFactory Factory { get; set; }

        public IEventStore EventStore { get; set; }

        public void Publish<TDomainEvent>(TDomainEvent e) where TDomainEvent : DomainEvent
        {
            ISubscribeTo<TDomainEvent>[] subscribers = Factory.GetSubscribersForEvent(e);

            foreach (var each in subscribers)
                each.Handle(e);

            EventStore.Append(e);
        }
    }
}