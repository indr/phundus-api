namespace Phundus.Tests
{
    using Common.Commanding;
    using Common.Domain.Model;
    using Machine.Specifications;

    public abstract class command_handler_concern<TCommand, THandler> : concern<THandler>
        where THandler : class, IHandleCommand<TCommand> where TCommand : ICommand
    {
        protected static InitiatorId theInitiatorId;
        protected static Initiator theInitiator;
        protected static TCommand command;

        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorId();
            theInitiator = new Initiator(theInitiatorId, "initiator@test.phundus.ch", "The Initiator");
        };

        public Because of = () =>
        {
            command.ShouldNotBeNull();
            if (catchException)
                caughtException = Catch.Exception(() => sut.Handle(command));
            else
                sut.Handle(command);
        };
    }
}