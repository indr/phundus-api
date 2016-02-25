namespace Phundus.Persistence.StoredEvents
{
    using Common.Eventing;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;

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
            Map(x => x.OccuredOnUtc, "OccuredOnUtc").Not.Update().CustomType<UtcDateTimeType>();
            Map(x => x.AggregateId, "AggregateId").Not.Update();
            Map(x => x.Serialization, "Serialization").Not.Update();
            Map(x => x.Version).Not.Update();
        }
    }
}