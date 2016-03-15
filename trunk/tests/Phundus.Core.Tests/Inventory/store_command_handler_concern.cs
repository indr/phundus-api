namespace Phundus.Tests.Inventory
{
    using Common.Commanding;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Model.Collaborators;
    using Phundus.Inventory.Model.Stores;

    public abstract class store_command_handler_concern<TCommand, THandler> :
        inventory_command_handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand>
        where TCommand : ICommand
    {
        protected static inventory_factory make;

        protected static IStoreRepository storeRepository;
        protected static ICollaboratorService userInRole;

        protected static Manager theManager;
        protected static Owner theOwner;
        protected static OwnerId theOwnerId;

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);

            userInRole = depends.on<ICollaboratorService>();
            storeRepository = depends.on<IStoreRepository>();

            theManager = make.Manager();
            theOwner = make.Owner();
            theOwnerId = theOwner.OwnerId;

            userInRole.setup(x => x.Manager(theInitiatorId, theOwnerId)).Return(theManager);
        };
    }
}