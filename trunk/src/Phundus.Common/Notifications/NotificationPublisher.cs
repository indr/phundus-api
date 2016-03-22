namespace Phundus.Common.Notifications
{
    using System;
    using System.Collections.Generic;
    using Castle.Core.Internal;
    using Eventing;
    using Messaging;

    public interface INotificationPublisher
    {
        void PublishNotification(StoredEvent storedEvent);
        void PublishNotification(IEnumerable<StoredEvent> storedEvents);
    }

    public static class NotificationPublisher
    {
        private static Func<INotificationPublisher> _factory;

        public static void Factory(Func<INotificationPublisher> factory)
        {
            _factory = factory;
        }

        public static void PublishNotification(StoredEvent storedEvent)
        {
            if (_factory == null)
                throw new InvalidOperationException(
                    "You need to provide a factory to the static NotificationPublisher class.");

            var publisher = _factory();
            publisher.PublishNotification(storedEvent);
        }
    }

    public class BusNotificationPublisher : INotificationPublisher
    {
        private readonly IBus _bus;
        private readonly IEventStore _eventStore;

        public BusNotificationPublisher(IBus bus, IEventStore eventStore)
        {
            _bus = bus;
            _eventStore = eventStore;
        }

        public void PublishNotification(StoredEvent storedEvent)
        {
            var notification = new Notification(storedEvent.EventId, _eventStore.Deserialize(storedEvent),
                storedEvent.Version);

            _bus.Send(notification);
        }

        public void PublishNotification(IEnumerable<StoredEvent> storedEvents)
        {
            storedEvents.ForEach(PublishNotification);
        }
    }

    public class InThreadNotificationPublisher : INotificationPublisher
    {
        private readonly IEventStore _eventStore;
        private readonly INotificationHandlerFactory _handlerFactory;

        public InThreadNotificationPublisher(INotificationHandlerFactory handlerFactory, IEventStore eventStore)
        {
            if (handlerFactory == null) throw new ArgumentNullException("handlerFactory");
            _handlerFactory = handlerFactory;
            _eventStore = eventStore;
        }

        public void PublishNotification(StoredEvent storedEvent)
        {
            var notification = new Notification(storedEvent.EventId, _eventStore.Deserialize(storedEvent),
                storedEvent.Version);

            PublishNotification(notification);
        }

        public void PublishNotification(IEnumerable<StoredEvent> storedEvents)
        {
            storedEvents.ForEach(PublishNotification);
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