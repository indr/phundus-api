namespace Phundus.Persistence.Dashboard
{
    using FluentNHibernate.Mapping;
    using Phundus.Dashboard.Querying.Records;

    public class EventLogRecordMap : ClassMap<EventLogRecord>
    {
        public EventLogRecordMap()
        {
            SchemaAction.All();
            Table("Es_Dashboard_EventLog");

            Id(x => x.EventGuid).GeneratedBy.Assigned();

            Map(x => x.Name);
            Map(x => x.OccuredOnUtc);
            Map(x => x.Text).Length(int.MaxValue);
        }
    }
}