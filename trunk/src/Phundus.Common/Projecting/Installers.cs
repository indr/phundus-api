namespace Phundus.Common.Projecting
{
    using System.Reflection;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Injecting;
    using Notifications;

    public class ProjectingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ITypedFactoryComponentSelector>().ImplementedBy<ProjectionSelector>(),
                Component.For<ITypedProjectionFactory>().AsFactory(c => c.SelectedWith<ProjectionSelector>()),
                Component.For<IProjectionFactory>().ImplementedBy<ProjectionFactory>());

            container.Register(
                Component.For<INotificationToConsumersDispatcher>().ImplementedBy<NotificationToConsumersDispatcher>(),
                Component.For<IStoredEventsProcessor>().ImplementedBy<StoredEventsProcessor>());
        }
    }

    public class ProjectionsInstaller : AssemblyInstallerBase
    {
        public override void Install(IWindsorContainer container, Assembly assembly)
        {
            container.Register(Classes.FromAssembly(assembly)
                .BasedOn<IEventConsumer>()
                .WithServiceBase()
                .LifestyleTransient());

            container.Register(Classes.FromAssembly(assembly)
                .BasedOn<ProjectionBase>()
                .WithServiceAllInterfaces());
        }
    }
}