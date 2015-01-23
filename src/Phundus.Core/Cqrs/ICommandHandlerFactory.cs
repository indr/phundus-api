namespace Phundus.Core.Cqrs
{
    using Common.Cqrs;

    public interface ICommandHandlerFactory
    {
        IHandleCommand<TCommand> GetHandlerForCommand<TCommand>(TCommand command) where TCommand : ICommand;

        IHandleCommand<TCommand>[] GetHandlersForCommand<TCommand>(TCommand command) where TCommand : ICommand;
    }
}