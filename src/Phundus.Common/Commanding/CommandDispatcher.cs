namespace Phundus.Common.Commanding
{
    using System;
    using Castle.Core.Logging;

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ILogger _logger;
        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public CommandDispatcher(ILogger logger, ICommandHandlerFactory commandHandlerFactory)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            if (commandHandlerFactory == null) throw new ArgumentNullException("commandHandlerFactory");
            _logger = logger;
            _commandHandlerFactory = commandHandlerFactory;
        }

        public void Dispatch<TCommand>(TCommand command)
        {
            _logger.Info("Dispatching command " + typeof(TCommand).Name);

            IHandleCommand<TCommand> handler = null;
            try
            {
                handler = _commandHandlerFactory.GetHandlerForCommand(command);
            }
            catch (Exception ex)
            {
                _logger.Error("Could not resolve command handler for " + typeof(TCommand).Name, ex);
                throw;
            }

            try
            {
                handler.Handle(command);
            }
            catch (Exception ex)
            {
                _logger.Error("Command handler threw exception", ex);
                throw;
            }
        }
    }
}