namespace Phundus.Core.Tests
{
    using System;
    using Core.Cqrs;
    using Machine.Specifications;

    public abstract class handler_concern<TCommand, THandler> : concern<THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static TCommand command;

        public Because of = () =>
        {
            if (Equals(command, default(TCommand)))
                throw new InvalidOperationException("In order to test a command handler, a command must be provided.");
            command.ShouldNotBeNull();
            sut.Handle(command);
        };
    }
}