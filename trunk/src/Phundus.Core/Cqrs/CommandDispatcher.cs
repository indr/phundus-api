namespace Phundus.Cqrs
{
    public class CommandDispatcher : ICommandDispatcher
    {
        public ICommandHandlerFactory Factory { get; set; }

        public void Dispatch<TCommand>(TCommand command)
        {
            IHandleCommand<TCommand> handler = Factory.GetHandlerForCommand(command);

            handler.Handle(command);
        }
    }
}