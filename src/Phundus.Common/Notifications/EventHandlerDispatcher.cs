namespace Phundus.Common.Notifications
{
    using System;
    using System.Reflection;
    using Castle.Core.Internal;
    using Castle.DynamicProxy;
    using Eventing;

    public interface IEventHandlerDispatcher
    {
        [Obsolete]
        void Update(string typeName);

        void Process(Notification notification);
        void ProcessMissedNotifications();
    }

    public class EventHandlerDispatcher : INotificationHandler, IEventHandlerDispatcher
    {
        private static readonly object Lock = new object();
        private readonly IEventHandlerFactory _eventHandlerFactory;
        private readonly IStoredEventsProcessor _storedEventsProcessor;
        private readonly IProcessedNotificationTrackerStore _trackerStore;

        public EventHandlerDispatcher(IEventHandlerFactory eventHandlerFactory,
            IStoredEventsProcessor storedEventsProcessor, IProcessedNotificationTrackerStore trackerStore)
        {
            _eventHandlerFactory = eventHandlerFactory;
            _storedEventsProcessor = storedEventsProcessor;
            _trackerStore = trackerStore;
        }

        [Obsolete]
        public void Update(string typeName)
        {
            var projection = _eventHandlerFactory.FindSubscriber(typeName);
            if (projection == null)
                return;

            UpdateHandler(projection);
        }

        public void Process(Notification notification)
        {
            lock (Lock)
            {
                MethodInfo method = typeof(IEventHandlerFactory).GetMethod("GetSubscribersForEvent");
                MethodInfo genericMethod = method.MakeGenericMethod(notification.Event.GetType());
                var subscribers = (ISubscribeTo[]) genericMethod.Invoke(_eventHandlerFactory, new object[] {notification.Event});
                //var subscribers = _eventHandlerFactory.GetSubscribersForEvent(notification.Event);
                subscribers.ForEach(UpdateHandler);
            }
        }

        public void Handle(Notification notification)
        {
            Process(notification);
        }

        public void ProcessMissedNotifications()
        {
            lock (Lock)
            {
                var subscribers = _eventHandlerFactory.GetSubscribers();
                subscribers.ForEach(UpdateHandler);
            }
        }

        private void UpdateHandler(ISubscribeTo eventHandler)
        {
            lock (Lock)
            {
                try
                {
                    var notDone = true;
                    while (notDone)
                    {
                        notDone = _storedEventsProcessor.Process(eventHandler);
                    }
                }
                catch (Exception ex)
                {
                    _trackerStore.TrackException(ProxyUtil.GetUnproxiedType(eventHandler).FullName, ex);
                }
            }
        }
    }
}