namespace Phundus.Common
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Commanding;
    using Eventing;
    using Notifications;

    public class CommonInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IDomainEventHandlerFactory>().AsFactory());

            container.Register(Component.For<IEventSerializer>().ImplementedBy<EventSerializer>());

            new CommandHandlerInstaller().Install(container, GetType().Assembly);
        }
    }
}