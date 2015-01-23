namespace Phundus.Core.Cqrs
{
    using Common.Cqrs;

    public class CommandDispatcher : ICommandDispatcher
    {
        public ICommandHandlerFactory Factory { get; set; }

        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            IHandleCommand<TCommand> handler = Factory.GetHandlerForCommand(command);

            handler.Handle(command);
        }
    }
}