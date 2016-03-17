namespace Phundus
{
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Infrastructure.Persistence;
    using Common.Projecting;
    using Common.Querying;
    using Inventory.Infrastructure.Persistence.Repositories;
    using Inventory.Model.Reservations;
    using Shop.Infrastructure;    
    using CollaboratorService = Inventory.Infrastructure.CollaboratorService;

    public class CoreInstaller : IWindsorInstaller
    {
        private readonly Assembly _assemblyContainingCommandsAndHandlers;

        public CoreInstaller() : this(typeof (CoreInstaller).Assembly)
        {
        }

        public CoreInstaller(Assembly assemblyContainingCommandsAndHandlers)
        {
            _assemblyContainingCommandsAndHandlers = assemblyContainingCommandsAndHandlers;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<QueryServiceBase>()
                    .WithServiceAllInterfaces());

            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<ProjectionBase>()
                    .WithServiceAllInterfaces());

            container.Register(
                Component.For<Inventory.Model.Collaborators.ICollaboratorService>()
                    .ImplementedBy<CollaboratorService>());

            container.Register(
                Component.For<Shop.Model.Collaborators.ICollaboratorService>()
                    .ImplementedBy<Shop.Infrastructure.CollaboratorService>());

            container.Register(Component.For<IWindsorContainer>().Instance(container));

            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("Service")).WithServiceDefaultInterfaces());
            container.Register(
                Classes.FromThisAssembly().Where(p => p.Name.EndsWith("Store")).WithServiceDefaultInterfaces());

            container.Register(
                Component.For<IOrderPdfFactory>().ImplementedBy<OrderPdfFactory>());

            container.Register(Component.For<IReservationRepository>()
                .ImplementedBy<NhReservationsBasedOnOrdersRepository>());

            container.Register(Classes.FromThisAssembly()
                .BasedOn(typeof (NhRepositoryBase<>))
                .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                .BasedOn<EventSourcedRepositoryBase>()
                .WithServiceAllInterfaces());
        }
    }
}