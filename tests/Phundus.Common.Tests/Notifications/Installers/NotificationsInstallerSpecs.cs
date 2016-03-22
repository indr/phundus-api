namespace Phundus.Common.Tests.Notifications.Installers
{
    using System.Reflection;
    using Common.Notifications;
    using Common.Notifications.Installers;
    using Machine.Specifications;

    public class when_installing_notifications_installer : windsor_installer_concern<NotificationsInstaller>
    {
        private Cleanup cleanup = () =>
            NotificationPublisher.Factory(null);

        private Establish ctx = () =>
            GetNotificationPublishersStaticFactory()
                .GetValue(null)
                .ShouldBeNull();

        private It should_set_notification_publishers_factory = () =>
            GetNotificationPublishersStaticFactory()
                .GetValue(null)
                .ShouldNotBeNull();

        private static FieldInfo GetNotificationPublishersStaticFactory()
        {
            var result = typeof (NotificationPublisher).GetField("_factory", BindingFlags.Static | BindingFlags.NonPublic);
            result.ShouldNotBeNull();
            return result;
        }
    }
}