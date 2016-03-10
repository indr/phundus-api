namespace Phundus.Persistence.Notifications.Mappings
{
    using Common.Notifications;
    using Extensions;
    using FluentNHibernate.Mapping;
    using NHibernate.Type;

    public class ProcessedNotificationTrackerMap : ClassMap<ProcessedNotificationTracker>
    {
        public ProcessedNotificationTrackerMap()
        {
            SchemaAction.Validate();

            Id(x => x.Id, "TrackerId").GeneratedBy.Assigned();
            Version(x => x.ConcurrencyVersion);

            Map(x => x.TypeName);
            Map(x => x.MostRecentProcessedNotificationId);
            Map(x => x.MostRecentProcessedAtUtc).CustomType<UtcDateTimeType>();

            Map(x => x.ErrorMessage).Nullable().WithMaxSize();
            Map(x => x.ErrorAtUtc).Nullable().CustomType<UtcDateTimeType>();
        }
    }
}