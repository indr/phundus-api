namespace Phundus.Persistence.StoredEvents
{
    using Common.Domain.Model;
    using Common.Events;
    using Common.Notifications;

    public interface INotificationPublisher
    {
        void PublishNotification(StoredEvent storedEvent, DomainEvent domainEvent);
    }

    public class InThreadNotificationPublisher : INotificationPublisher
    {
        public INotificationListenerFactory Factory { get; set; }

        public void PublishNotification(StoredEvent storedEvent, DomainEvent domainEvent)
        {
            var listeners = Factory.GetNotificationListeners();

            foreach (var eachListener in listeners)
                eachListener.Handle(storedEvent, domainEvent);
        }
    }

    public interface INotificationListenerFactory
    {
        INotificationListener[] GetNotificationListeners();
    }
}