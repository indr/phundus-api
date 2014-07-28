namespace Phundus.Core.Ddd
{
    public class EventPublisherImpl
    {
        public IEventHandlerFactory Factory { get; set; }

        public void Publish<TDomainEvent>(TDomainEvent @event)
        {
            ISubscribeTo<TDomainEvent>[] subscribers = Factory.GetSubscribersForEvent(@event);

            foreach (var each in subscribers)
                each.Handle(@event);
        }
    }
}