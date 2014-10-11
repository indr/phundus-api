namespace Phundus.Persistence.Tests.Ddd
{
    using System;
    using Core.Ddd;
    using Machine.Fakes;
    using Machine.Specifications;
    using NHibernate;
    using Persistence.Ddd;
    using Rhino.Mocks;

    public class TestDomainEvent : DomainEvent
    {
        
    }

    [Subject(typeof(EventStore))]
    public class when_a_domain_event_is_appended : concern<EventStore>
    {
        protected Establish c = () =>
        {
            repository = depends.on<IStoredEventRepository>();
            depends.on<IEventSerializer>();
        };

        public Because of = () => sut.Append(new TestDomainEvent());

        public It should_add_to_repository =
            () => repository.WasToldTo(x => x.Add(Arg<StoredEvent>.Is.NotNull));

        private static IStoredEventRepository repository;
    }
}
