namespace Phundus.Common.Projecting.Installers
{
    using System.Reflection;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Notifications.Installers;

    public class ProjectingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ITypedFactoryComponentSelector>().ImplementedBy<ProjectionSelector>(),
                Component.For<ITypedProjectionFactory>().AsFactory(c => c.SelectedWith<ProjectionSelector>()),
                Component.For<IProjectionFactory>().ImplementedBy<ProjectionFactory>());            
        }
    }
}