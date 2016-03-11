namespace Phundus.Common.Eventing
{
    using Domain.Model;

    public interface IEventPublisher
    {
        void Publish<TDomainEvent>(TDomainEvent e) where TDomainEvent : DomainEvent;
    }

    public class EventPublisherImpl : IEventPublisher
    {
        private readonly IEventStore _eventStore;        

        public EventPublisherImpl(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Publish<TDomainEvent>(TDomainEvent e) where TDomainEvent : DomainEvent
        {
            //ISubscribeTo<TDomainEvent>[] subscribers = Factory.GetSubscribersForEvent(e);

            //foreach (var each in subscribers)
            //    each.Handle(e);

            _eventStore.Append(e);
        }
    }
}