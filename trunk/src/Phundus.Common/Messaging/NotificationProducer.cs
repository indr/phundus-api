namespace Phundus.Common.Messaging
{
    using EventPublishing;
    using Events;
    using Notifications;

    public class NotificationProducer : INotificationProducer
    {
        public NotificationProducer(IEventStore eventStore, IEventPublisher eventPublisher)
        {
            EventStore = eventStore;
            EventPublisher = eventPublisher;
        }

        public IEventStore EventStore { get; set; }

        public INotificationConsumerFactory Factory { get; set; }

        public IEventPublisher EventPublisher { get; set; }

        public void Produce(StoredEvent storedEvent)
        {
            var domainEvent = EventStore.ToDomainEvent(storedEvent);
            var notification = new Notification(storedEvent.EventId, domainEvent,
                storedEvent.StreamName, storedEvent.StreamVersion);

            new NotificationDispatcher(Factory).Dispatch(notification);

            // TODO: Right place to do this?
            EventPublisher.Publish((dynamic)domainEvent, false);
        }
    }
}