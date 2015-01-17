namespace Phundus.Common
{
    using EventPublishing;
    using Messaging;
    using Notifications;

    public class SubscribeToDispatcher : INotificationConsumer
    {
        private readonly IEventHandlerFactory _eventHandlerFactory;

        public SubscribeToDispatcher(IEventHandlerFactory eventHandlerFactory)
        {
            _eventHandlerFactory = eventHandlerFactory;
        }

        private void Publish<TDomainEvent>(TDomainEvent domainEvent)
        {
            ISubscribeTo<TDomainEvent>[] subscribers = _eventHandlerFactory.GetSubscribersForEvent(domainEvent);

            foreach (var each in subscribers)
                each.Handle(domainEvent);
        }

        public void Consume(Notification notification)
        {
            Publish((dynamic)notification.Event);
        }
    }
}