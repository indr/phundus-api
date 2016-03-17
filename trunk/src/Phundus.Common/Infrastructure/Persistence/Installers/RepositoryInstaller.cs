namespace Phundus.Common.Infrastructure.Persistence.Installers
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Notifications;
    using Phundus.Persistence.Notifications.Repositories;

    public class RepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                .BasedOn(typeof (NhRepositoryBase<>))
                .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                .BasedOn<EventSourcedRepositoryBase>()
                .WithServiceAllInterfaces());

            

            container.Register(Component.For<ITrackerRepository>()
                .ImplementedBy<NhTrackerRepository>());
        }
    }
}