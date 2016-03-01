namespace Phundus.Common.Commanding
{
    public interface ICommandHandlerFactory
    {
        IHandleCommand<TCommand> GetHandlerForCommand<TCommand>(TCommand command);

        IHandleCommand<TCommand>[] GetHandlersForCommand<TCommand>(TCommand command);
    }
}