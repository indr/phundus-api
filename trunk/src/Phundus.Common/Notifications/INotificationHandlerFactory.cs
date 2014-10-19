namespace Phundus.Common.Notifications
{
    public interface INotificationHandlerFactory
    {
        INotificationHandler[] GetNotificationHandlers();
    }
}