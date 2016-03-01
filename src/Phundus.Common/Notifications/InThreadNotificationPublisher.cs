namespace Phundus.Common.Notifications
{
    using System;
    using System.Collections.Generic;
    using Eventing;

    public class InThreadNotificationPublisher : INotificationPublisher
    {
        private readonly IEventStore _eventStore;
        private readonly INotificationConsumerFactory _consumerFactory;

        public InThreadNotificationPublisher(IEventStore eventStore, INotificationConsumerFactory consumerFactory)
        {
            if (eventStore == null) throw new ArgumentNullException("eventStore");
            if (consumerFactory == null) throw new ArgumentNullException("consumerFactory");

            _eventStore = eventStore;
            _consumerFactory = consumerFactory;
        }

        public void PublishNotification(StoredEvent storedEvent)
        {
            var notification = new Notification(storedEvent.EventId, _eventStore.Deserialize(storedEvent), storedEvent.Version);

            PublishNotification(notification);
        }

        public void PublishNotification(IEnumerable<StoredEvent> storedEvents)
        {
            foreach (var each in storedEvents)
                PublishNotification(each);
        }

        private void PublishNotification(Notification notification)
        {
            var handlers = _consumerFactory.GetNotificationConsumers();

            foreach (var each in handlers)
            {
                each.Handle(notification);
            }
        }
    }
}