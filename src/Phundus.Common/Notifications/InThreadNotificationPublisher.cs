namespace Phundus.Common.Notifications
{
    using Eventing;    

    public class InThreadNotificationPublisher : INotificationPublisher
    {
        public IEventStore EventStore { get; set; }

        public INotificationHandlerFactory Factory { get; set; }

        public InThreadNotificationPublisher(IEventStore eventStore)
        {
            EventStore = eventStore;
        }

        public void PublishNotification(StoredEvent storedEvent)
        {
            var notification = new Notification(storedEvent.EventId, EventStore.Deserialize(storedEvent));

            var handlers = Factory.GetNotificationHandlers();
            
            foreach (var each in handlers)
            {
                each.Handle(notification);
            }
        }
    }
}