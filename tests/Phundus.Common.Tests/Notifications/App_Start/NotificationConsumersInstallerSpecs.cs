namespace Phundus.Common.Tests.Notifications.App_Start
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
    public class NotificationConsumersInstallerSpecs :
        assembly_installer_concern<NotificationHandlerInstaller, NotificationConsumersInstallerSpecs>
    {
        private It should_resolve_to_TestNotificationConsumer = () =>
            resolve<INotificationHandler>().ShouldBeOfExactType<TestNotificationHandler>();
    }
}