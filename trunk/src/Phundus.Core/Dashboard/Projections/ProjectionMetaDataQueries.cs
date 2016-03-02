namespace Phundus.Dashboard.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Notifications;
    using Common.Querying;
    using Newtonsoft.Json;

    public interface IProjectionsMetaDataQueries
    {
        ICollection<ProjectionMetaData> Query();
    }

    public class ProjectionsMetaDataQueries : QueryBase<ProjectionMetaData>, IProjectionsMetaDataQueries
    {
        private readonly IProcessedNotificationTrackerStore _processedNotificationTrackerStore;

        public ProjectionsMetaDataQueries(IProcessedNotificationTrackerStore processedNotificationTrackerStore)
        {
            if (processedNotificationTrackerStore == null)
                throw new ArgumentNullException("processedNotificationTrackerStore");
            _processedNotificationTrackerStore = processedNotificationTrackerStore;
        }

        public ICollection<ProjectionMetaData> Query()
        {
            var trackers = _processedNotificationTrackerStore.GetProcessedNotificationTrackers();
            return trackers.Select(s => new ProjectionMetaData
            {
                ProjectionId = s.TypeName,
                Name = s.TypeName,
                ProcessedEventId = s.MostRecentProcessedNotificationId
            }).ToList();
        }
    }

    public class ProjectionMetaData
    {
        [JsonProperty("projectionId")]
        public string ProjectionId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("processedEventId")]
        public long ProcessedEventId { get; set; }
    }
}