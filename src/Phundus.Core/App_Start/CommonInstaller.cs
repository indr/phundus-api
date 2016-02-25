namespace Phundus
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Eventing;
    using Common.Notifications;

    public class CommonInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Types.FromThisAssembly().BasedOn<INotificationHandler>().WithServiceFromInterface());

            container.Register(Component.For<IDomainEventHandlerFactory>().AsFactory());

            container.Register(Component.For<INotificationLogFactory>().ImplementedBy<NotificationLogFactory>());
            container.Register(Component.For<IEventSerializer>().ImplementedBy<EventSerializer>());
        }
    }
}