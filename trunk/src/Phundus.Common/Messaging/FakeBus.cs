namespace Phundus.Common.Messaging
{
    using System;
    using System.Threading;
    using Castle.Core.Logging;
    using Castle.Windsor;
    using Commanding;
    using Notifications;

    public class FakeBus : IBus
    {
        private readonly IWindsorContainer _container;

        public FakeBus(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }

        public void Send<T>(T message)
        {
            if (message is ICommand)
            {
                SendCommand(message);
                return;
            }
            if (message is Notification)
            {
                SendNotification(message as Notification);
                return;
            }

            throw new InvalidOperationException("Unknown message " + message.GetType().FullName);
        }

        private void SendNotification(Notification message)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                var consumers = _container.Resolve<INotificationConsumerFactory>().GetNotificationConsumers();
                var logger = _container.Resolve<ILogger>();
                foreach (var each in consumers)
                {
                    try
                    {
                        each.Handle(message);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal("Notification consumer threw exception.", ex);
                    }
                }
            });
        }

        private void SendCommand<T>(T message)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                var handler = _container.Resolve<ICommandHandlerFactory>().GetHandlerForCommand(message);
                var logger = _container.Resolve<ILogger>();
                try
                {
                    handler.Handle(message);
                }
                catch (Exception ex)
                {
                    logger.Fatal("Command handler threw exception.", ex);
                }
            });
        }
    }
}