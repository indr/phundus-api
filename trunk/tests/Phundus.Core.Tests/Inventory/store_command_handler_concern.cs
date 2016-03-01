namespace Phundus.Tests.Inventory
{
    using Common.Commanding;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Stores.Repositories;

    public abstract class store_command_handler_concern<TCommand, THandler> :
        command_handler_concern<TCommand, THandler> where THandler : class, IHandleCommand<TCommand>
    {
        protected static inventory_factory make;

        protected static IStoreRepository storeRepository;
        protected static IUserInRole userInRole;

        protected static Manager theManager;
        protected static Owner theOwner;

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);

            userInRole = depends.on<IUserInRole>();
            storeRepository = depends.on<IStoreRepository>();

            theManager = make.Manager();
            theOwner = make.Owner();

            userInRole.setup(x => x.Manager(theInitiatorId, theOwner.OwnerId)).Return(theManager);
        };
    }
}