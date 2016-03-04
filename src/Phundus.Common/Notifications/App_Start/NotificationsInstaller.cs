namespace Phundus.Common.Notifications.App_Start
{
    using System.Reflection;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class NotificationsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<INotificationLogFactory>()
                .ImplementedBy<NotificationLogFactory>());

            container.Register(Component.For<INotificationPublisher>()
                //.ImplementedBy<InThreadNotificationPublisher>());
                .ImplementedBy<BusNotificationPublisher>());

            container.Register(Component.For<INotificationConsumerFactory>()
                .AsFactory());

            new NotificationConsumersInstaller().Install(container, Assembly.GetExecutingAssembly());
        }
    }
}