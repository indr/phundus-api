namespace Phundus.Persistence.Notifications
{
    using Common;
    using Common.Notifications;
    using Extensions;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;

    public class ProcessedNotificationTrackerMap : ClassMap<ProcessedNotificationTracker>
    {
        public ProcessedNotificationTrackerMap()
        {
            SchemaAction.None();

            Id(x => x.Id, "TrackerId").GeneratedBy.Assigned();
            Version(x => x.ConcurrencyVersion);

            Map(x => x.TypeName);
            Map(x => x.MostRecentProcessedNotificationId);

            Map(x => x.LastProcessingAtUtc).CustomType<UtcDateTimeType>();
            Map(x => x.ErrorMessage).WithMaxSize();
        }
    }
}