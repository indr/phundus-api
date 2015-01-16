namespace Phundus.Core
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Notifications;
    using Common.Port.Adapter.Persistence;

    public class QueriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("Service")).WithServiceDefaultInterfaces());
            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("Store")).WithServiceDefaultInterfaces());

            // TODO: Sollte entfernt werden
            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("ReadModel")).WithServiceAllInterfaces());
        }
    }

    public class ProjectionInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly().BasedOn(typeof (NHibernateProjectionBase)).WithServiceAllInterfaces());
        }
    }

    public class QueryServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly().BasedOn(typeof (NHibernateQueryServiceBase<>)).WithServiceDefaultInterfaces());
        }
    }
}