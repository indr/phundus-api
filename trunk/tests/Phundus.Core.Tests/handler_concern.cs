namespace Phundus.Tests
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;
    using Phundus.Cqrs;

    public abstract class handler_concern<TCommand, THandler> : concern<THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static InitiatorId theInitiatorId;
        protected static TCommand command;

        protected static Exception caughtException;
        protected static bool catchException = false;
        private Establish ctx = () => theInitiatorId = new InitiatorId();

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