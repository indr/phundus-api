namespace Phundus.Common.Messaging
{
    using System;
    using System.Threading;
    using Castle.Core.Logging;
    using Commanding;

    public class FakeBus : IBus
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILogger _logger;

        public FakeBus(ILogger logger, ICommandDispatcher commandDispatcher)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            if (commandDispatcher == null) throw new ArgumentNullException("commandDispatcher");
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        public void Send<T>(T message)
        {
            if (message is ICommand)
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    try
                    {
                        _commandDispatcher.Dispatch(message);
                    }
                    catch (Exception ex)
                    {
                        _logger.Fatal("Command dispatcher threw exception.", ex);
                    }
                });
                return;
            }


            throw new InvalidOperationException("Unknown message.");
        }
    }
}