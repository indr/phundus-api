namespace Phundus.Rest.ContentObjects
{
    using Newtonsoft.Json;

    public class CoordinateCto
    {
        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }
    }
}