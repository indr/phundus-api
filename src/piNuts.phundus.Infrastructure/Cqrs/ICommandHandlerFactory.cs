namespace Phundus.Infrastructure.Cqrs
{
    public interface ICommandHandlerFactory
    {
        IHandleCommand[] GetHandlersForCommand(ICommand command);

        IHandleCommand<TCommand> GetHandlerForCommand<TCommand>(TCommand command);
    }
}