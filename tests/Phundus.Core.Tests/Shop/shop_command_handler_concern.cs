namespace Phundus.Tests.Shop
{
    using Common.Commanding;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using Phundus.Shop.Model.Collaborators;

    public abstract class shop_command_handler_concern<TCommand, THandler> : command_handler_concern<TCommand, THandler>
        where THandler : class, IHandleCommand<TCommand> where TCommand : ICommand
    {
        protected static shop_factory make;

        protected static ICollaboratorService collaboratorService;

        private Establish ctx = () =>
        {
            make = new shop_factory(fake);

            collaboratorService = depends.on<ICollaboratorService>();
            collaboratorService.setup(x => x.Initiator(theInitiatorId)).Return(new Initiator(theInitiatorId, "initiator@test.phundus.ch", "The Initiator"));
        };
    }
}