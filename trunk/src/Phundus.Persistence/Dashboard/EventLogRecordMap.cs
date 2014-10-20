namespace Phundus.Persistence.Dashboard
{
    using Core.Dashboard.Querying.Records;
    using FluentNHibernate.Mapping;

    public class EventLogRecordMap : ClassMap<EventLogRecord>
    {
        public EventLogRecordMap()
        {
            SchemaAction.All();
            Table("Rm_EventLog");

            Id(x => x.EventGuid).GeneratedBy.Assigned();

            Map(x => x.Name);
            Map(x => x.OccuredOnUtc);
            Map(x => x.Text).Length(int.MaxValue);
        }
    }
}