namespace Phundus.Rest.ContentObjects
{
    using Newtonsoft.Json;

    public class Coordinate
    {
        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }

        public static Coordinate FromLatLng(decimal? latitude, decimal? longitude)
        {
            if (!latitude.HasValue || !longitude.HasValue)
                return null;
            return new Coordinate
            {
                Latitude = latitude.Value,
                Longitude = longitude.Value
            };
        }
    }
}