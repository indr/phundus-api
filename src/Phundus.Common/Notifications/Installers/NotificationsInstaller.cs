namespace Phundus.Common.Notifications.Installers
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
            NotificationPublisher.Factory(container.Resolve<INotificationPublisher>);

            container.Register(
                Component.For<INotificationHandlerFactory>().AsFactory());

            container.Register(                
                Component.For<IStoredEventsProcessor>().ImplementedBy<StoredEventsProcessor>());

            new NotificationHandlerInstaller().Install(container, Assembly.GetExecutingAssembly());            
        }
    }
}