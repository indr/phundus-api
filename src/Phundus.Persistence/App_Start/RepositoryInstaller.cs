namespace Phundus.Persistence
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Notifications;
    using Inventory.Repositories;
    using Notifications.Repositories;
    using Phundus.Inventory.Model.Reservations;

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

            container.Register(Component.For<IReservationRepository>()
                .ImplementedBy<NhReservationsBasedOnOrdersRepository>());

            container.Register(Component.For<ITrackerRepository>()
                .ImplementedBy<NhTrackerRepository>());
        }
    }
}