namespace Phundus.Persistence
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Notifications;
    using Inventory.Repositories;
    using Notifications;
    using Phundus.Inventory.Model.Reservations;
    using Shop.Repositories;
    using StoredEvents;

    public class TrackerStoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IProcessedNotificationTrackerStore>()
                .ImplementedBy<ProcessedNotificationTrackerStore>()
                .LifestyleTransient());
        }
    }

    public class PersistenceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IEventStore>().ImplementedBy<EventStore>());

            container.Register(Classes.FromThisAssembly()
                .BasedOn(typeof (NhRepositoryBase<>))
                .WithServiceAllInterfaces()
                .LifestyleTransient());

            container.Register(Classes.FromThisAssembly()
                .BasedOn<EventSourcedRepositoryBase>()
                .WithServiceAllInterfaces()
                .LifestyleTransient());

            container.Register(Component.For<IReservationRepository>()
                .ImplementedBy<NhReservationsBasedOnOrdersRepository>()
                .LifestyleTransient());

            container.Register(Component.For<IShortIdGeneratorService>()
                .ImplementedBy<ShortIdGeneratorService>());
        }
    }
}