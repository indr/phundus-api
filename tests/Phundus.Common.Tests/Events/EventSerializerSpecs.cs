namespace Phundus.Common.Tests.Events
{
    using System.Runtime.Serialization;
    using Common.Events;
    using developwithpassion.specifications.rhinomocks;
    using Domain.Model;
    using Machine.Specifications;

    [DataContract]
    public class FakeDomainEvent : DomainEvent
    {
        [DataMember(Order = 1)]
        public string FirstName { get; set; }
    }

    [Subject(typeof (EventSerializer))]
    public class when_a_domain_event_is_serialized : Observes<EventSerializer>
    {
        private static FakeDomainEvent domainEvent;
        private static byte[] serialization;

        public Establish c = () =>
        {
            domainEvent = new FakeDomainEvent();
            domainEvent.FirstName = "Hans";
        };

        public Because of = () => { serialization = sut.Serialize(domainEvent); };

        public It should_return_serialization = () => serialization.ShouldNotBeEmpty();
    }

    [Subject(typeof (EventSerializer))]
    public class when_a_domain_event_is_serialized_and_deserialized : Observes<EventSerializer>
    {
        private static FakeDomainEvent domainEvent;
        private static FakeDomainEvent deserialization;

        public Establish c = () =>
        {
            domainEvent = new FakeDomainEvent();
            domainEvent.FirstName = "Peter";
        };

        public Because of = () =>
        {
            var serialization = sut.Serialize(domainEvent);
            deserialization = sut.Deserialize<FakeDomainEvent>(domainEvent.Id, domainEvent.OccuredOnUtc, serialization);
        };

        public It should_have_equal_id = () => deserialization.Id.ShouldEqual(domainEvent.Id);

        public It should_have_same_first_name =
            () => deserialization.FirstName.ShouldEqual(domainEvent.FirstName);

        public It should_have_same_occured_on_utc =
            () => deserialization.OccuredOnUtc.ShouldEqual(domainEvent.OccuredOnUtc);
    }
}