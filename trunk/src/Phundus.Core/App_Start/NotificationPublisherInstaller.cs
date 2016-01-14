namespace Phundus
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Notifications;

    public class NotificationPublisherInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<INotificationPublisher>()
                    .ImplementedBy<InThreadNotificationPublisher>()
                    .LifestyleTransient());

            container.Register(Component.For<INotificationHandlerFactory>().AsFactory());
        }
    }
}