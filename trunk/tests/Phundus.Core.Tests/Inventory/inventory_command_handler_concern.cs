namespace Phundus.Tests.Inventory
{
    using Common.Commanding;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Model;
    using Phundus.Inventory.Model.Collaborators;

    public abstract class inventory_command_handler_concern<TCommand, THandler> :
        command_handler_concern<TCommand, THandler> where TCommand : ICommand
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static inventory_factory make;

        protected static ICollaboratorService collaboratorService;

        protected static OwnerId theOwnerId;
        protected static Owner theOwner;

        protected static Manager theManager;
        

        private Establish ctx = () =>
        {
            make = new inventory_factory(fake);

            theOwner = make.Owner();
            theOwnerId = theOwner.OwnerId;
            

            theManager = make.Manager(theInitiatorId);
            

            collaboratorService = depends.on<ICollaboratorService>();
            collaboratorService.setup(x => x.Manager(theInitiatorId, theOwnerId)).Return(theManager);
        };
    }
}