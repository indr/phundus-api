namespace Phundus.Persistence.StoredEvents
{
    using Common.Events;
    using FluentNHibernate.Mapping;

    public class StoredEventMap : ClassMap<StoredEvent>
    {
        public StoredEventMap()
        {
            ReadOnly();
            Table("StoredEvents");

            Id(x => x.EventId, "EventId").GeneratedBy.Native();
            Map(x => x.EventGuid, "EventGuid").Not.Update();
            Map(x => x.TypeName, "TypeName").Not.Update();
            Map(x => x.OccuredOnUtc, "OccuredOnUtc").Not.Update();
            Map(x => x.AggregateId, "AggregateId").Not.Update();
            Map(x => x.Serialization, "Serialization").Not.Update();
        }
    }
}