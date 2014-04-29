namespace Phundus.Core.Common
{
    public class CommandDispatcher : ICommandDispatcher
    {
        public ICommandHandlerFactory Factory { get; set; }

        public void Dispatch(ICommand command)
        {
            var handlers = Factory.GetHandlersForCommand(command);

            foreach (var each in handlers)
            {
                each.Execute();
            }
        }
    }
}