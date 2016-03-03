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
        private string _status = "success";

        [JsonProperty("projectionId")]
        public string ProjectionId { get; set; }

        [JsonProperty("status")]
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("processedEventId")]
        public long ProcessedEventId { get; set; }

        [JsonProperty("behind")]
        public long Behind { get; set; }

        public void SetBehind(long maxEventId)
        {
            Behind = maxEventId - ProcessedEventId;
            if (Status == "success" && Behind > 0)
            {
                Status = "warning";
            }
        }
    }
}