namespace Phundus.Common.Tests.Notifications.Installers
{
    using System;
    using Common.Notifications;
    using Common.Notifications.Installers;
    using Machine.Specifications;

    public class TestNotificationHandler : INotificationHandler
    {
        public void Handle(Notification notification)
        {
            throw new NotImplementedException();
        }

        public void ProcessMissedNotifications()
        {
            throw new NotImplementedException();
        }
    }

    [Subject(typeof (NotificationHandlerInstaller))]
    public class when_installing_notification_handler_installer :
        assembly_installer_concern<NotificationHandlerInstaller, when_installing_notification_handler_installer>
    {
        private It should_resolve_to_TestNotificationHandler = () =>
            resolve<INotificationHandler>().ShouldBeOfExactType<TestNotificationHandler>();
    }
}