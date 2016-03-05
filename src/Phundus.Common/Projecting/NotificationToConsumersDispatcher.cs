namespace Phundus.Common.Projecting
{
    using System;
    using Castle.Core.Internal;
    using Notifications;

    public interface INotificationToConsumersDispatcher
    {
        [Obsolete]
        void Update(string typeName);
        void Process(Notification notification);
        void ProcessMissedNotifications();
    }

    public class NotificationToConsumersDispatcher : INotificationToConsumersDispatcher
    {
        private static readonly object Lock = new object();
        private readonly IEventConsumerFactory _consumerFactory;
        private readonly IStoredEventsProcessor _storedEventsProcessor;
        private readonly IProcessedNotificationTrackerStore _trackerStore;

        public NotificationToConsumersDispatcher(IEventConsumerFactory consumerFactory, IStoredEventsProcessor storedEventsProcessor,
            IProcessedNotificationTrackerStore trackerStore)
        {
            if (consumerFactory == null) throw new ArgumentNullException("consumerFactory");
            if (storedEventsProcessor == null) throw new ArgumentNullException("storedEventsProcessor");
            if (trackerStore == null) throw new ArgumentNullException("trackerStore");
            _consumerFactory = consumerFactory;
            _storedEventsProcessor = storedEventsProcessor;
            _trackerStore = trackerStore;
        }

        [Obsolete]
        public void Update(string typeName)
        {
            var projection = _consumerFactory.FindConsumer(typeName);
            if (projection == null)
                return;

            UpdateConsumer(projection);
        }

        public void Process(Notification notification)
        {
            UpdateConsumers();
        }

        public void ProcessMissedNotifications()
        {
            UpdateConsumers();
        }

        private void UpdateConsumers()
        {
            lock (Lock)
            {
                var consumers = _consumerFactory.GetConsumers();
                consumers.ForEach(UpdateConsumer);
            }
        }

        private void UpdateConsumer(IEventConsumer consumer)
        {
            lock (Lock)
            {
                try
                {
                    var notDone = true;
                    while (notDone)
                    {
                        notDone = _storedEventsProcessor.Process(consumer);
                    }
                }
                catch (Exception ex)
                {
                    _trackerStore.TrackException(consumer.GetType().FullName, ex);
                }
            }
        }
    }
}