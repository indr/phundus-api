namespace Phundus.Persistence.Dashboard
{
    using Core.Dashboard.Querying.Records;
    using FluentNHibernate.Mapping;

    public class EventsRecordMap : ClassMap<EventsRecord>
    {
        public EventsRecordMap()
        {
            SchemaAction.All();
            Table("Rm_Events");

            Id(x => x.EventGuid).GeneratedBy.Assigned();

            Map(x => x.Name);
            Map(x => x.OccuredOnUtc);
            Map(x => x.Text).Length(int.MaxValue);
        }
    }
}