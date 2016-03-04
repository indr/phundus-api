namespace Phundus.Common.Notifications
{
    using System;
    using System.Collections.Generic;
    using Castle.Core.Internal;
    using Domain.Model;
    using Eventing;
    using Messaging;

    public interface INotificationPublisher
    {
        void PublishNotification(StoredEvent storedEvent, Func<StoredEvent, DomainEvent> deserializer);
        void PublishNotification(IEnumerable<StoredEvent> storedEvents, Func<StoredEvent, DomainEvent> deserializer);
    }

    public class BusNotificationPublisher : INotificationPublisher
    {
        private readonly IBus _bus;        

        public BusNotificationPublisher(IBus bus)
        {
            if (bus == null) throw new ArgumentNullException("bus");
            _bus = bus;
        }

        public void PublishNotification(StoredEvent storedEvent, Func<StoredEvent, DomainEvent> deserializer)
        {
            var notification = new Notification(storedEvent.EventId, deserializer(storedEvent), storedEvent.Version);

            _bus.Send(notification);
        }

        public void PublishNotification(IEnumerable<StoredEvent> storedEvents, Func<StoredEvent, DomainEvent> deserializer)
        {
            storedEvents.ForEach(each => PublishNotification(each,deserializer));
        }
    }

    public class InThreadNotificationPublisher : INotificationPublisher
    {
        private readonly INotificationConsumerFactory _consumerFactory;

        public InThreadNotificationPublisher(INotificationConsumerFactory consumerFactory)
        {
            if (consumerFactory == null) throw new ArgumentNullException("consumerFactory");
            _consumerFactory = consumerFactory;
        }

        public void PublishNotification(StoredEvent storedEvent, Func<StoredEvent, DomainEvent> deserializer)
        {
            var notification = new Notification(storedEvent.EventId, deserializer(storedEvent),
                storedEvent.Version);

            PublishNotification(notification);
        }

        public void PublishNotification(IEnumerable<StoredEvent> storedEvents, Func<StoredEvent, DomainEvent> deserializer)
        {
            foreach (var each in storedEvents)
                PublishNotification(each, deserializer);
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