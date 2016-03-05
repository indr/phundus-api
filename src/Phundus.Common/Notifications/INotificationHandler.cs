namespace Phundus.Common.Notifications
{
    public interface INotificationHandler
    {
        void Handle(Notification notification);

        void ProcessMissedNotifications();
    }
}