namespace Phundus.Persistence
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using StoredEvents;

    public class NotificationPublisherInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<INotificationPublisher>()
                    .ImplementedBy<InThreadNotificationPublisher>()
                    .LifestyleTransient());

            container.Register(Component.For<INotificationListenerFactory>().AsFactory());
        }
    }
}