namespace Phundus.Common.Tests.Projecting
{
    using System;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Notifications;
    using Common.Projecting;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Rhino.Mocks;

    public interface ITestConsumer : IConsumes<DomainEvent>
    {
    }


    public class TestConsumerAdapter : ITestConsumer
    {
        // RedirectToWhen does not work with castle proxies

        private readonly IConsumes<DomainEvent> _mock;

        public TestConsumerAdapter(IConsumes<DomainEvent> mock)
        {
            _mock = mock;
        }

        public void Consume(DomainEvent e)
        {
            _mock.Consume(e);
        }
    }

    public class projection_updater_concern : Observes<StoredEventsProcessor>
    {
        protected static IEventStore eventStore;
        protected static IProcessedNotificationTrackerStore trackerStore;

        protected static DomainEvent domainEvent1;
        protected static DomainEvent domainEvent2;
        protected static long lowNotificationId = 123;
        protected static long maxNotificationId = lowNotificationId + 2;
        protected static ProcessedNotificationTracker tracker;
        protected static ITestConsumer consumer;
        protected static ITestConsumer consumerMock;
        protected static IEventSerializer eventSerializer = new EventSerializer();

        

        private Establish ctx = () =>
        {
            domainEvent1 = new DomainEvent();
            domainEvent2 = new DomainEvent();
            tracker = new ProcessedNotificationTracker("typeName");
            tracker.Track(lowNotificationId);
            consumerMock = fake.an<ITestConsumer>();
            consumer = new TestConsumerAdapter(consumerMock);

            eventStore = depends.on<IEventStore>();
            eventStore.setup(x => x.GetMaxNotificationId()).Return(maxNotificationId);
            var storedEvents = new[] {makeStoredEvent(domainEvent1), makeStoredEvent(domainEvent2)};
            eventStore.setup(x => x.AllStoredEventsBetween(lowNotificationId + 1, maxNotificationId))
                .Return(storedEvents);
            eventStore.setup(x => x.Deserialize(storedEvents[0])).Return(domainEvent1);
            eventStore.setup(x => x.Deserialize(storedEvents[1])).Return(domainEvent2);

            trackerStore = depends.on<IProcessedNotificationTrackerStore>();
            trackerStore.setup(x => x.GetProcessedNotificationTracker(consumer.GetType().FullName)).Return(tracker);
        };

        private static StoredEvent makeStoredEvent(DomainEvent domainEvent)
        {
            return new StoredEvent(Guid.NewGuid(), DateTime.UtcNow, domainEvent.GetType().AssemblyQualifiedName,
                new byte[0], Guid.Empty);
        }
    }

    [Subject(typeof (StoredEventsProcessor))]
    public class when_projection_updater_updates_projection : projection_updater_concern
    {
        private Because of = () =>
            sut.Process(consumer);

        private It should_tell_consumer_to_consume = () =>
        {
            consumerMock.received(x => x.Consume(domainEvent1));
            consumerMock.received(x => x.Consume(domainEvent2));
        };

        private It should_track_processed_notification_id = () =>
            trackerStore.received(x => x.TrackMostRecentProcessedNotificationId(tracker, maxNotificationId));
    }

    [Subject(typeof (StoredEventsProcessor))]
    public class when_max_notification_id_is_equal_to_most_recent_processed_notification_id : projection_updater_concern
    {
        private static bool returnValue;

        private Establish ctx = () =>
            tracker.Track(maxNotificationId);

        private Because of = () =>
            returnValue = sut.Process(consumer);

        private It should_return_false = () =>
            returnValue.ShouldBeFalse();
    }

    [Subject(typeof (StoredEventsProcessor))]
    public class when_difference_to_max_notification_id_is_greater_than_notifications_per_update :
        projection_updater_concern
    {
        private static bool returnValue;

        private Establish ctx = () =>
        {
            tracker.Track(0);
            eventStore.setup(x => x.AllStoredEventsBetween(Arg<long>.Is.Anything, Arg<long>.Is.Anything))
                .Return(new StoredEvent[0]);
        };

        private Because of = () =>
            returnValue = sut.Process(consumer);

        private It should_return_true = () =>
            returnValue.ShouldBeTrue();
    }

    [Subject(typeof (StoredEventsProcessor))]
    public class when_difference_to_max_notification_id_is_equal_to_notifications_per_update :
        projection_updater_concern
    {
        private static bool returnValue;

        private Establish ctx = () =>
        {
            tracker.Track(maxNotificationId - StoredEventsProcessor.NotificationsPerUpdate);
            eventStore.setup(x => x.AllStoredEventsBetween(Arg<long>.Is.Anything, Arg<long>.Is.Anything))
                .Return(new StoredEvent[0]);
        };

        private Because of = () =>
            returnValue = sut.Process(consumer);

        private It should_return_false = () =>
            returnValue.ShouldBeFalse();
    }
}