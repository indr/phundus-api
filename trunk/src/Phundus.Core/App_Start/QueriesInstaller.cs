namespace Phundus.Core
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Cqrs;

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

            //container.Register(Classes.FromThisAssembly().BasedOn<ReadModelBase>().WithServiceAllInterfaces());
            
            container.Register(Classes.FromThisAssembly().BasedOn(typeof (NHibernateReadModelBase<>)).WithServiceAllInterfaces());
        }
    }
}