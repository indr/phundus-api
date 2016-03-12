namespace Phundus.Common.Eventing.Installers
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class EventingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ITypedFactoryComponentSelector>().ImplementedBy<EventHandlerSelector>(),
                Component.For<AutoReleaseEventHandlerInterceptor>(),
                Component.For<IEventPublisher>().ImplementedBy<EventPublisherImpl>(),
                Component.For<IEventHandlerFactory>().AsFactory(c => c.SelectedWith<EventHandlerSelector>()));

            EventPublisher.Factory(container.Resolve<IEventPublisher>);
        }
    }
}