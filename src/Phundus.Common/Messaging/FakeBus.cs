namespace Phundus.Common.Messaging
{
    using System;
    using System.Threading;
    using Castle.Windsor;
    using Commanding;
    using Elmah;
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

        private void SendNotification(Notification notification)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                var handlers = _container.Resolve<INotificationHandlerFactory>().GetNotificationHandlers();
                foreach (var each in handlers)
                {
                    try
                    {
                        each.Handle(notification);
                    }
                    catch (Exception ex)
                    {
                        Elmah.ErrorLog.GetDefault(null).Log(new Error(ex));
                    }
                }
            });
        }

        private void SendCommand<T>(T message)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                var handler = _container.Resolve<ICommandHandlerFactory>().GetHandlerForCommand(message);
                try
                {
                    handler.Handle(message);
                }
                catch (Exception ex)
                {
                    Elmah.ErrorLog.GetDefault(null).Log(new Error(ex));
                }
            });
        }
    }
}