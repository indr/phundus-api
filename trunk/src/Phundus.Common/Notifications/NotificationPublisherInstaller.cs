namespace Phundus.Common
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Notifications;

    public class NotificationPublisherInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<INotificationLogFactory>()
                .ImplementedBy<NotificationLogFactory>());

            container.Register(Component.For<INotificationPublisher>()
                    .ImplementedBy<InThreadNotificationPublisher>());
        }
    }
}