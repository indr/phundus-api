namespace Phundus.Tests.Inventory
{
    using Common.Commanding;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Inventory.Model.Collaborators;

    public abstract class inventory_command_handler_concern<TCommand, THandler> :
        command_handler_concern<TCommand, THandler> where TCommand : ICommand where THandler : class, IHandleCommand<TCommand>
    {
        protected static ICollaboratorService collaboratorService;

        private Establish ctx = () =>
        {
            collaboratorService = depends.on<ICollaboratorService>();
            collaboratorService.setup(x => x.Initiator(theInitiatorId)).Return(new Initiator(theInitiatorId, "initiator@test.phundus.ch", "The Initiator"));
        };
    }
}