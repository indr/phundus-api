namespace Phundus.Common.Projecting
{
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

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