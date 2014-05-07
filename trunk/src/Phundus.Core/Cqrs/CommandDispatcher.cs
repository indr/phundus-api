namespace Phundus.Core.Cqrs
{
    using Castle.Windsor;

    public class CommandDispatcher : ICommandDispatcher
    {
        public ICommandHandlerFactory Factory { get; set; }

        public IWindsorContainer Container { get; set; }

        public void Dispatch<TCommand>(TCommand command)
        {
            IHandleCommand<TCommand>[] handlers = Container.ResolveAll<IHandleCommand<TCommand>>();
                //Factory.GetHandlersForCommand<TCommand>();

            foreach (var each in handlers)
            {
                each.Handle(command);
            }
        }
    }
}