namespace Phundus.Core
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Inventory.Port.Adapter.Persistence;

    public class EventStoreRepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                .BasedOn<EventStoreRepositoryBase>().WithServiceDefaultInterfaces());
        }
    }
}