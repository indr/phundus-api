namespace Phundus.Common.Notifications
{
    using Events;

    public interface INotificationPublisher
    {
        void PublishNotification(StoredEvent storedEvent);
    }
}