namespace Phundus.Tests.Inventory
{
    using Common.Commanding;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Model.Collaborators;
    using Phundus.Inventory.Model.Stores;

    public abstract class store_command_handler_concern<TCommand, THandler> :
        inventory_command_handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
        where TCommand : ICommand
    {
        protected static IStoreRepository storeRepository;
        protected static ICollaboratorService userInRole;

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);

            userInRole = depends.on<ICollaboratorService>();
            storeRepository = depends.on<IStoreRepository>();

            userInRole.setup(x => x.Manager(theInitiatorId, theOwnerId)).Return(theManager);
        };
    }
}