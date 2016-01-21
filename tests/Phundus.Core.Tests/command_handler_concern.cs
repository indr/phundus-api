namespace Phundus.Tests
{
    using System;
    using Authorization;
    using Common.Domain.Model;
    using Integration.IdentityAccess;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Cqrs;

    public abstract class command_handler_concern<TCommand, THandler> : concern<THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static InitiatorId theInitiatorId;
        protected static Initiator theInitiator;
        protected static TCommand command;

        protected static IAuthorize authorize;
        protected static IInitiatorService initiatorService;

        protected static Exception caughtException;
        protected static bool catchException = false;
        
        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorId();
            theInitiator = new Initiator(theInitiatorId, "initiator@test.phundus.ch", "The Initiator");

            authorize = depends.on<IAuthorize>();
            initiatorService = depends.on<IInitiatorService>();
            initiatorService.WhenToldTo(x => x.GetActiveById(theInitiatorId)).Return(theInitiator);
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