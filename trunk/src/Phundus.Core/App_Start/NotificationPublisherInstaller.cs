namespace Phundus.Core
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Messaging;
    using Common.Notifications;

    public class NotificationPublisherInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<INotificationProducer>()
                    .ImplementedBy<NotificationProducer>()
                    .LifestyleTransient());

            container.Register(Component.For<INotificationConsumerFactory>().AsFactory());
        }
    }
}