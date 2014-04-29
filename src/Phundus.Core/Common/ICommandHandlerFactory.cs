namespace Phundus.Core.Common
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler[] GetHandlersForCommand(ICommand command);
    }
}