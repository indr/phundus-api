namespace Phundus.Core.Cqrs
{
    public interface ICommandHandlerFactory
    {
        IHandleCommand[] GetHandlersForCommand(ICommand command);
    }
}