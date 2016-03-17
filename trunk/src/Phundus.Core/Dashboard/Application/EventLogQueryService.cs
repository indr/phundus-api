namespace Phundus.Dashboard.Application
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Common.Projecting;
    using Common.Querying;
    using Newtonsoft.Json;

    public interface IEventLogQueryService
    {
        IEnumerable<EventLogData> Query();
    }

    public class EventLogQueryService : QueryServiceBase<EventLogData>, IEventLogQueryService
    {
        public IEnumerable<EventLogData> Query()
        {
            return QueryOver().OrderBy(p => p.OccuredOnUtc).Desc.Take(20).List();
        }
    }

    public class EventLogData
    {
        [JsonProperty("eventGuid")]
        public virtual Guid EventGuid { get; set; }

        [JsonProperty("occuredOnUtc")]
        public virtual DateTime OccuredOnUtc { get; set; }

        [JsonProperty("type")]
        public virtual string Name { get; set; }

        [JsonProperty("data")]
        [JsonConverter(typeof (RawJsonConverter))]
        public virtual string JsonData { get; protected set; }

        public virtual string Text { get; set; }

        public virtual void SetData(object data)
        {
            var stringWriter = new StringWriter();
            var settings = new JsonSerializerSettings();
            JsonSerializer.Create(settings).Serialize(stringWriter, data);
            JsonData = stringWriter.GetStringBuilder().ToString();
        }
    }
}