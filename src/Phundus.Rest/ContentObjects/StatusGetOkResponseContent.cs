namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class StatusGetOkResponseContent
    {
        [JsonProperty("serverDateTimeUtc")]
        public DateTime ServerDateTimeUtc { get; set; }

        [JsonProperty("serverUrl")]
        public string ServerUrl { get; set; }

        [JsonProperty("serverVersion")]
        public string ServerVersion { get; set; }

        [JsonProperty("inMaintenance")]
        public bool InMaintenance { get; set; }
    }
}