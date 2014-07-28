namespace Phundus.Core.Ddd
{
    public class EventPublisherImpl
    {
        public IEventHandlerFactory Factory { get; set; }

        public void Publish<TDomainEvent>(TDomainEvent @event)
        {
            ISubscribeTo<TDomainEvent> subscriber = Factory.GetSubscriberForEvent(@event);

            subscriber.Handle(@event);
        }
    }
}