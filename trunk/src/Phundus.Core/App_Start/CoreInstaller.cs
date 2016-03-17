namespace Phundus
{
    using System.Reflection;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Common.Projecting;
    using Common.Querying;
    using Inventory.Infrastructure;
    using Shop.Model.Pdf;

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
                    .BasedOn<QueryBase>()
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
                Component.For<IOrderPdfGenerator>().ImplementedBy<OrderPdfGenerator>());
        }
    }
}