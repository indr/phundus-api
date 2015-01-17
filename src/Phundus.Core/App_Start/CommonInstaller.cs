namespace Phundus.Core
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common;
    using Common.Events;
    using Common.Messaging;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence.View;

    public class CommonInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<INotificationConsumer>().ImplementedBy<ProjectionDispatcher>());
            container.Register(Component.For<INotificationConsumer>().ImplementedBy<SubscribeToDispatcher>());

            container.Register(Component.For<IDomainEventHandlerFactory>().AsFactory());

            container.Register(Component.For<INotificationLogFactory>().ImplementedBy<NotificationLogFactory>());
            container.Register(Component.For<IEventSerializer>().ImplementedBy<EventSerializer>());
        }
    }
}