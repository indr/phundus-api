namespace Phundus.Common.Notifications
{
    public interface INotificationLogFactory
    {
        NotificationLog CreateCurrentNotificationLog();
        NotificationLog CreateNotificationLog(string notificationLogId);
    }
}