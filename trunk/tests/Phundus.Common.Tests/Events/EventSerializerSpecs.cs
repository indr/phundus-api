namespace Phundus.Common.Tests.Events
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Common.Events;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    [DataContract]
    public class TestDomainEvent : DomainEvent
    {
        [DataMember(Order = 1)]
        public string FirstName { get; set; }
    }

    [DataContract]
    public class TestDomainEventWithCustomType : DomainEvent
    {
        [DataMember(Order = 1)]
        public string ClrType { get; set; }

        [DataMember(Order = 2)]
        public CustomType CustomType { get; set; }
    }

    [DataContract]
    public class TestDomainEventWithCollectionOfCustomType : DomainEvent
    {
        [DataMember(Order = 1)]
        public ICollection<CustomType> Items { get; set; } 
    }

    [DataContract]
    public class CustomType
    {
        [DataMember(Order = 1)]
        public string CustomTypesValue { get; set; }
    }

    [Subject(typeof (EventSerializer))]
    public class when_a_domain_event_is_serialized : Observes<EventSerializer>
    {
        private static TestDomainEvent domainEvent;
        private static byte[] serialization;

        private Establish ctx = () =>
        {
            domainEvent = new TestDomainEvent();
            domainEvent.FirstName = "Hans";
        };

        private Because of = () => { serialization = sut.Serialize(domainEvent); };

        private It should_return_serialization = () => serialization.ShouldNotBeEmpty();
    }

    [Subject(typeof (EventSerializer))]
    public class when_a_domain_event_is_serialized_and_deserialized : Observes<EventSerializer>
    {
        private static TestDomainEvent domainEvent;
        private static TestDomainEvent deserialization;

        private Establish c = () =>
        {
            domainEvent = new TestDomainEvent();
            domainEvent.FirstName = "Peter";
        };

        private Because of = () =>
        {
            var serialization = sut.Serialize(domainEvent);
            deserialization = sut.Deserialize<TestDomainEvent>(domainEvent.EventGuid, domainEvent.OccuredOnUtc, serialization);
        };

        private It should_have_equal_id = () => deserialization.EventGuid.ShouldEqual(domainEvent.EventGuid);

        private It should_have_same_first_name =
            () => deserialization.FirstName.ShouldEqual(domainEvent.FirstName);

        private It should_have_same_occured_on_utc =
            () => deserialization.OccuredOnUtc.ShouldEqual(domainEvent.OccuredOnUtc);
    }

    [Subject(typeof (EventSerializer))]
    public class when_a_domain_event_with_custom_type_is_serialized_and_deserialized : Observes<EventSerializer>
    {
        private static TestDomainEventWithCustomType domainEvent;
        private static TestDomainEventWithCustomType deserialized;

        private Establish ctx = () =>
        {
            domainEvent = new TestDomainEventWithCustomType();
            domainEvent.ClrType = "ClrTypes value";
            domainEvent.CustomType = new CustomType();
            domainEvent.CustomType.CustomTypesValue = "CustomTypes value";
        };

        private Because of = () =>
        {
            var serialized = sut.Serialize(domainEvent);
            deserialized = sut.Deserialize<TestDomainEventWithCustomType>(domainEvent.EventGuid,
                domainEvent.OccuredOnUtc, serialized);
        };

        private It should_have_equal_clr_types_value = () => deserialized.ClrType.ShouldEqual("ClrTypes value");

        private It should_have_equal_custom_types_value =
            () => deserialized.CustomType.CustomTypesValue.ShouldEqual("CustomTypes value");
    }

    [Subject(typeof (EventSerializer))]
    public class when_a_domain_event_with_a_collection_of_custom_types_is_serialized_and_deserialized :
        Observes<EventSerializer>
    {
        private static TestDomainEventWithCollectionOfCustomType domainEvent;
        private static TestDomainEventWithCollectionOfCustomType deserialized;

        private Establish ctx = () =>
        {
            domainEvent = new TestDomainEventWithCollectionOfCustomType();
            domainEvent.Items = new List<CustomType>
            {
                new CustomType {CustomTypesValue = "Item 1"},
                new CustomType {CustomTypesValue = "Item 2"}
            };
        };

        private Because of = () =>
        {
            var serialized = sut.Serialize(domainEvent);
            deserialized = sut.Deserialize<TestDomainEventWithCollectionOfCustomType>(domainEvent.EventGuid,
                domainEvent.OccuredOnUtc, serialized);
        };

        private It should_have_equal_items_count =
            () => deserialized.Items.Count.ShouldEqual(2);

        private It should_contain_items_with_equal_values =
            () => deserialized.Items.ShouldContain(c => c.CustomTypesValue == "Item 1" || c.CustomTypesValue == "Item 2");
    }
}