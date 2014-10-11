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
            session = depends.on<ISession>();
            depends.on<Func<ISession>>().WhenToldTo(x => x.Invoke()).Return(session);
            depends.on<IEventSerializer>();
        };

        public Because of = () => sut.Append(new TestDomainEvent());

        public It should_save_a_new_stored_event_to_session =
            () => session.WasToldTo(x => x.Save(Arg<StoredEvent>.Is.NotNull));

        private static ISession session;
    }
}
