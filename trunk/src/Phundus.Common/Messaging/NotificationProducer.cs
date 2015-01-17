namespace Phundus.Common.Messaging
{
    using Events;
    using Notifications;

    public class NotificationProducer : INotificationProducer
    {
        private readonly INotificationConsumerFactory _notificationConsumerFactory;

        public NotificationProducer(INotificationConsumerFactory notificationConsumerFactory)
        {
            _notificationConsumerFactory = notificationConsumerFactory;
        }       

        public void Produce(IEventStore eventStore, StoredEvent storedEvent)
        {
            var domainEvent = eventStore.ToDomainEvent(storedEvent);
            var notification = new Notification(storedEvent.EventId, domainEvent,
                storedEvent.StreamName, storedEvent.StreamVersion);

            new NotificationDispatcher(_notificationConsumerFactory).Dispatch(notification);
        }
    }
}