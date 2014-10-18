namespace Phundus.Persistence.Dashboard
{
    using Core.Dashboard.Querying.Records;
    using FluentNHibernate.Mapping;

    public class EventsListViewRecordMap : ClassMap<EventsListViewRecord>
    {
        public EventsListViewRecordMap()
        {
            SchemaAction.All();
            Table("Rm_EventsListView");

            Id(x => x.EventGuid).GeneratedBy.Assigned();

            Map(x => x.Name);
            Map(x => x.OccuredOnUtc);
        }
    }
}