namespace Phundus.Common
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Notifications;
    using Projecting;

    public class NotificationConsumerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<INotificationConsumer>()
                .ImplementedBy<ProjectionDispatcher>());
            
            container.Register(Component.For<INotificationConsumerFactory>()
                .AsFactory());
        }
    }
}