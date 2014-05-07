namespace Phundus.Core.Cqrs
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler[] GetHandlersForCommand(ICommand command);
    }
}