namespace Phundus.Core.Tests.InstallerTests
{
    using Core.Inventory.Domain.Model.Management;
    using Core.Inventory.Domain.Model.Reservations;
    using Core.Inventory.Port.Adapter.Persistence;
    using Machine.Specifications;

    [Subject(typeof (EventStoreRepositoryInstaller))]
    public class when_event_store_repository_installer_is_installed : installer_concern<EventStoreRepositoryInstaller>
    {
        public It should_resolve_IReservationRepository =
            () => Container.Resolve<IReservationRepository>().ShouldBeOfExactType<EventStoreReservationRepository>();

        public It should_resolve_IStockRepository =
            () => Container.Resolve<IStockRepository>().ShouldBeOfExactType<EventStoreStockRepository>();
    }
}