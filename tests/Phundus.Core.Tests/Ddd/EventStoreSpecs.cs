namespace Phundus.Core.Tests.Ddd
{
    using Common.Domain.Model;
    using Common.Events;
    using Core.Ddd;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Ddd;
    using Rhino.Mocks;

    public class TestDomainEvent : DomainEvent
    {
    }

    [Subject(typeof (EventStore))]
    public class when_a_domain_event_is_appended :  Observes<EventStore>
    {
        private static IStoredEventRepository repository;

        protected Establish c = () =>
        {
            repository = depends.on<IStoredEventRepository>();
            depends.on<IEventSerializer>();
        };

        public Because of = () => sut.Append(new TestDomainEvent());

        public It should_add_to_repository =
            () => repository.WasToldTo(x => x.Append(Arg<StoredEvent>.Is.NotNull));
    }
}