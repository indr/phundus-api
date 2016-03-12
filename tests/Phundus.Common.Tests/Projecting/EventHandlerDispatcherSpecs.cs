namespace Phundus.Common.Tests.Projecting
{
    using System;
    using Castle.DynamicProxy;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Notifications;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class event_handler_dispatcher_concern : Observes<EventHandlerDispatcher>
    {
        protected static IEventHandlerFactory handlerFactory;
        protected static IStoredEventsProcessor projectionUpdater;
        protected static IProcessedNotificationTrackerStore trackerStore;

        protected static ISubscribeTo<DomainEvent> eventHandler1;
        protected static ISubscribeTo<DomainEvent> eventHandler2;

        private Establish ctx = () =>
        {
            eventHandler1 = fake.an<ISubscribeTo<DomainEvent>>();
            eventHandler2 = fake.an<ISubscribeTo<DomainEvent>>();

            handlerFactory = depends.on<IEventHandlerFactory>();
            handlerFactory.setup(x => x.GetSubscribers()).Return(new[] {eventHandler1, eventHandler2});
            handlerFactory.setup(x => x.GetSubscribersForEventNonGeneric(Arg<DomainEvent>.Is.Anything)).Return(new[] {eventHandler1, eventHandler2});

            projectionUpdater = depends.on<IStoredEventsProcessor>();
            trackerStore = depends.on<IProcessedNotificationTrackerStore>();
        };
    }

    [Subject(typeof (EventHandlerDispatcher))]
    public class when_updating : event_handler_dispatcher_concern
    {
        private Establish ctx = () =>
            handlerFactory.setup(x => x.GetSubscriber("typeName")).Return(eventHandler1);

        private Because of = () =>
            sut.Force("typeName");

        private It should_tell_projection_update = () =>
            projectionUpdater.received(x => x.Process(eventHandler1));
    }

    [Subject(typeof (EventHandlerDispatcher))]
    public class when_projection_updater_updates_returns_true_as_not_done : event_handler_dispatcher_concern
    {
        private static int calls;

        private Establish ctx = () =>
        {
            handlerFactory.setup(x => x.GetSubscriber("typeName")).Return(eventHandler1);
            projectionUpdater.WhenToldTo(x => x.Process(eventHandler1)).Return(() => ++calls < 2);
        };

        private Because of = () =>
            sut.Force("typeName");

        private It should_call_update_twice = () =>
            projectionUpdater.received(x => x.Process(eventHandler1)).Twice();
    }

    [Subject(typeof (EventHandlerDispatcher))]
    public class when_projection_update_throws_exception : event_handler_dispatcher_concern
    {
        private static Exception exception;

        private Establish ctx = () =>
        {
            exception = new Exception("Error message");
            handlerFactory.setup(x => x.GetSubscriber(eventHandler1.GetType().FullName)).Return(eventHandler1);
            projectionUpdater.setup(x => x.Process(eventHandler1)).Throw(exception);
        };

        private Because of = () =>
            sut.Force(eventHandler1.GetType().FullName);

        private It should_track_exception = () =>
            trackerStore.received(x => x.TrackException(eventHandler1.GetType().FullName, exception));
    }

    [Subject(typeof (EventHandlerDispatcher))]
    public class when_handling_notification : event_handler_dispatcher_concern
    {
        private Because of = () =>
            sut.Process(new Notification(1234, new DomainEvent(), 1));

        private It should_tell_projection_update = () =>
        {
            projectionUpdater.received(x => x.Process(eventHandler1));
            projectionUpdater.received(x => x.Process(eventHandler2));
        };
    }


    [Subject(typeof (EventHandlerDispatcher))]
    public class when_projection_processor_processes_missed_notification_ : event_handler_dispatcher_concern
    {
        private Because of = () =>
            sut.ProcessMissedNotifications();

        private It should_tell_projection_update = () =>
        {
            projectionUpdater.received(x => x.Process(eventHandler1));
            projectionUpdater.received(x => x.Process(eventHandler2));
        };
    }
}