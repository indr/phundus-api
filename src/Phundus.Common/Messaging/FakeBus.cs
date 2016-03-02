namespace Phundus.Common.Messaging
{
    using System;
    using System.Threading;
    using Commanding;

    public class FakeBus : IBus
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public FakeBus(ICommandDispatcher commandDispatcher)
        {
            if (commandDispatcher == null) throw new ArgumentNullException("commandDispatcher");

            _commandDispatcher = commandDispatcher;
        }

        public void Send<T>(T message)
        {
            if (message is ICommand)
            {
                ThreadPool.QueueUserWorkItem(o => _commandDispatcher.Dispatch(message));
                return;
            }
            

            throw new InvalidOperationException("Unknown message.");
        }
    }
}