namespace Phundus.Core.Tests
{
    using Core.Cqrs;
    using Machine.Specifications;

    public abstract class handler_concern<TCommand, THandler> : concern<THandler>
        where THandler : class, IHandleCommand<TCommand>
    {
        protected static TCommand command;

        public Because of = () =>
        {
            command.ShouldNotBeNull();
            sut.Handle(command);
        };
    }
}