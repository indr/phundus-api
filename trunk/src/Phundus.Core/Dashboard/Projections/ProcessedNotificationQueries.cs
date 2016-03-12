namespace Phundus.Dashboard.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Notifications;
    using Common.Querying;
    using Newtonsoft.Json;

    public interface IEventProcessorsQueries
    {
        ICollection<EventProcessorData> Query();
    }

    public class EventProcessorsQueries : QueryBase<EventProcessorData>, IEventProcessorsQueries
    {
        private readonly IProcessedNotificationTrackerStore _processedNotificationTrackerStore;

        public EventProcessorsQueries(IProcessedNotificationTrackerStore processedNotificationTrackerStore)
        {
            _processedNotificationTrackerStore = processedNotificationTrackerStore;
        }

        public ICollection<EventProcessorData> Query()
        {
            var trackers = _processedNotificationTrackerStore.GetProcessedNotificationTrackers();
            return trackers.Select(s => new EventProcessorData
            {
                ProcessorId = s.TypeName,
                TypeName = s.TypeName,
                IsProjection = s.TypeName.EndsWith("Projection"),
                ProcessedEventId = s.MostRecentProcessedNotificationId,
                ProcessedAtUtc = s.MostRecentProcessedAtUtc,
                ErrorMessage = s.ErrorMessage,
                ErrorAtUtc = s.ErrorAtUtc
            }).ToList();
        }
    }

    public class EventProcessorData
    {
        private string _errorMessage;
        private string _status = "success";
        private string _typeName;

        [JsonProperty("processorId")]
        public string ProcessorId { get; set; }

        [JsonProperty("status")]
        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        [JsonProperty("typeName")]
        public string TypeName
        {
            get { return _typeName; }
            set
            {
                _typeName = value;
                if (String.IsNullOrWhiteSpace(_typeName))
                    return;
                var parts = _typeName.Split('.');
                if (parts.Length < 3)
                    return;
                Context = parts[1];
                Name = parts.LastOrDefault();
            }
        }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("processedEventId")]
        public long ProcessedEventId { get; set; }

        [JsonProperty("processedAtUtc")]
        public DateTime? ProcessedAtUtc { get; set; }

        [JsonProperty("behind")]
        public long Behind { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                if (_errorMessage != null)
                    _status = "danger";
            }
        }

        [JsonProperty("errorAtUtc")]
        public DateTime? ErrorAtUtc { get; set; }

        [JsonProperty("isProjection")]
        public bool IsProjection { get; set; }

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