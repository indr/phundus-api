namespace Phundus.Common.Notifications
{
    public interface INotificationConsumer
    {
        void Handle(Notification notification);

        void ProcessMissedNotifications();
    }
}