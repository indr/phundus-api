namespace Phundus.Common.Tests.Notifications.App_Start
{
    using System;
    using Common.Notifications;
    using Common.Notifications.App_Start;
    using Machine.Specifications;

    public class TestNotificationConsumer : INotificationConsumer
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

    [Subject(typeof (NotificationConsumersInstaller))]
    public class NotificationConsumersInstallerSpecs :
        assembly_installer_concern<NotificationConsumersInstaller, NotificationConsumersInstallerSpecs>
    {
        private It should_resolve_to_TestNotificationConsumer = () =>
            resolve<INotificationConsumer>().ShouldBeOfExactType<TestNotificationConsumer>();
    }
}