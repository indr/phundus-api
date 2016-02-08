namespace Phundus.Common.Projections
{
    using System;
    using System.IO;
    using Newtonsoft.Json;

    public class ActionsProjectionRowBase
    {
        [JsonProperty("eventGuid")]
        public virtual Guid EventGuid { get; set; }

        [JsonProperty("occuredOnUtc")]
        public virtual DateTime OccuredOnUtc { get; set; }

        [JsonProperty("type")]
        public virtual string Name { get; set; }

        public virtual void SetData(object data)
        {
            var stringWriter = new StringWriter();
            var settings = new JsonSerializerSettings();
            JsonSerializer.Create(settings).Serialize(stringWriter, data);
            JsonData = stringWriter.GetStringBuilder().ToString();
        }

        [JsonProperty("data")]
        [JsonConverter(typeof(RawJsonConverter))]
        public virtual string JsonData { get; protected set; }
    }
}