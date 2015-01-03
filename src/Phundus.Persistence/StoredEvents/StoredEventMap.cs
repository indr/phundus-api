namespace Phundus.Persistence.StoredEvents
{
    using Common.Events;
    using FluentNHibernate.Mapping;

    public class SagaStoredEventMap : ClassMap<SagaStoredEvent>
    {
        public SagaStoredEventMap()
        {
            SchemaAction.Validate();

            ReadOnly();
            Table("SagaStoredEvents");

            Id(x => x.EventId, "EventId").GeneratedBy.Native();
            Map(x => x.EventGuid, "EventGuid").Not.Update();
            Map(x => x.TypeName, "TypeName").Not.Update();
            Map(x => x.OccuredOnUtc, "OccuredOnUtc").Not.Update();
            Map(x => x.Serialization, "Serialization").Not.Update();

            Map(x => x.StreamName, "StreamName").Not.Update();
            Map(x => x.StreamVersion, "StreamVersion").Not.Update();
        }
    }

    public class StoredEventMap : ClassMap<StoredEvent>
    {
        public StoredEventMap()
        {
            SchemaAction.Validate();

            ReadOnly();
            Table("StoredEvents");

            Id(x => x.EventId, "EventId").GeneratedBy.Native();
            Map(x => x.EventGuid, "EventGuid").Not.Update();
            Map(x => x.TypeName, "TypeName").Not.Update();
            Map(x => x.OccuredOnUtc, "OccuredOnUtc").Not.Update();
            Map(x => x.Serialization, "Serialization").Not.Update();

            Map(x => x.StreamName, "StreamName").Not.Update();
            Map(x => x.StreamVersion, "StreamVersion").Not.Update();
        }
    }
}