namespace Phundus.Persistence.Tests.Ddd
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Notifications;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class event_store_concern : concern<EventStore>
    {
        protected static IStoredEventRepository repository;
        protected static INotificationPublisher notificationPublisher;

        protected Establish c = () =>
        {
            repository = depends.on<IStoredEventRepository>();
            depends.on<IEventSerializer>();
            notificationPublisher = depends.on<INotificationPublisher>();
        };
    }

    public class TestDomainEvent : DomainEvent
    {
    }

    public class TestId : GuidIdentity
    {
    }

    [Subject(typeof (EventStore))]
    public class when_appending : event_store_concern
    {
        public Because of = () =>
            sut.Append(new TestDomainEvent());

        public It should_add_to_repository =
            () => repository.WasToldTo(x => x.Append(Arg<StoredEvent>.Is.NotNull));

        private It should_not_publish_notification = () =>
            notificationPublisher.never_received(
                x => x.PublishNotification(Arg<StoredEvent>.Is.Anything));
    }

    [Subject(typeof (EventStore))]
    public class when_appending_to_stream : event_store_concern
    {
        private Because of = () =>
            sut.AppendToStream(new TestId(), 1, new[] {new TestDomainEvent(), new TestDomainEvent()});

        private It should_not_publish_notification = () =>
            notificationPublisher.never_received(
                x => x.PublishNotification(Arg<IEnumerable<StoredEvent>>.Is.Anything));
    }
}