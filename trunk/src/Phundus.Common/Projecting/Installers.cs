namespace Phundus.Common.Projecting
{
    using System.Reflection;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Injecting;

    public class ProjectingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ITypedFactoryComponentSelector>().ImplementedBy<ProjectionSelector>(),
                Component.For<ITypedProjectionFactory>().AsFactory(c => c.SelectedWith<ProjectionSelector>()),
                Component.For<IProjectionFactory>().ImplementedBy<ProjectionFactory>());


            container.Register(                
                Component.For<IProjectionUpdater>().ImplementedBy<ProjectionUpdater>(),
                Component.For<IProjectionProcessor>().ImplementedBy<ProjectionProcessor>());
        }
    }

    public class ProjectionsInstaller : AssemblyInstallerBase
    {
        public override void Install(IWindsorContainer container, Assembly assembly)
        {
            container.Register(Types.FromAssembly(assembly)
                .BasedOn<ProjectionBase>()
                .WithServiceAllInterfaces());
        }
    }
}