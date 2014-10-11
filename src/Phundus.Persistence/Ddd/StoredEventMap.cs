namespace Phundus.Persistence.Ddd
{
    using Core.Ddd;
    using FluentNHibernate.Mapping;

    public class StoredEventMap : ClassMap<StoredEvent>
    {
        public StoredEventMap()
        {
            ReadOnly();
            Table("StoredEvents");

            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.EventId, "EventGuid").Not.Update();
            Map(x => x.Name, "TypeName").Not.Update();
            Map(x => x.OccuredOnUtc, "OccuredOnUtc").Not.Update();
            Map(x => x.AggregateId, "AggregateId").Not.Update();
            Map(x => x.Serialization, "Serialization").Not.Update();
        }
    }
}