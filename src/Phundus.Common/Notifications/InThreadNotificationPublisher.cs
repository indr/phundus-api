namespace Phundus.Common.Notifications
{
    using System;
    using System.Collections.Generic;
    using Eventing;

    public class InThreadNotificationPublisher : INotificationPublisher
    {
        private readonly IEventStore _eventStore;
        private readonly INotificationHandlerFactory _handlerFactory;

        public InThreadNotificationPublisher(IEventStore eventStore, INotificationHandlerFactory handlerFactory)
        {
            if (eventStore == null) throw new ArgumentNullException("eventStore");
            if (handlerFactory == null) throw new ArgumentNullException("handlerFactory");

            _eventStore = eventStore;
            _handlerFactory = handlerFactory;
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
            var handlers = _handlerFactory.GetNotificationHandlers();

            foreach (var each in handlers)
            {
                each.Handle(notification);
            }
        }
    }
}