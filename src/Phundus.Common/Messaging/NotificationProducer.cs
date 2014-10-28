namespace Phundus.Common.Messaging
{
    using Events;
    using Notifications;

    public class NotificationProducer : INotificationProducer
    {
        public NotificationProducer(IEventStore eventStore)
        {
            EventStore = eventStore;
        }

        public IEventStore EventStore { get; set; }

        public INotificationConsumerFactory Factory { get; set; }

        public void Produce(StoredEvent storedEvent)
        {
            var notification = new Notification(storedEvent.EventId, EventStore.ToDomainEvent(storedEvent),
                storedEvent.StreamName, storedEvent.StreamVersion);

            new NotificationDispatcher(Factory).Dispatch(notification);
        }
    }
}