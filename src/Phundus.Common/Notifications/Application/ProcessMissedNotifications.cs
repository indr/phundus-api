namespace Phundus.Common.Notifications.Application
{
    using Commanding;

    public class ProcessMissedNotifications : ICommand
    {
    }

    public class ProcessMissedNotificationsHandler : IHandleCommand<ProcessMissedNotifications>
    {
        private readonly INotificationHandlerFactory _notificationHandlerFactory;

        public ProcessMissedNotificationsHandler(INotificationHandlerFactory notificationHandlerFactory)
        {
            _notificationHandlerFactory = notificationHandlerFactory;
        }

        public void Handle(ProcessMissedNotifications command)
        {
            var handlers = _notificationHandlerFactory.GetNotificationHandlers();
            foreach (var handler in handlers)
            {
                handler.ProcessMissedNotifications();
            }
        }
    }
}