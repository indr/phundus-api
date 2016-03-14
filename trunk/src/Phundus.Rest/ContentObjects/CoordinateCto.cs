namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class CoordinateCto
    {
        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }

        [Obsolete("Use AutoMapper")]
        public static CoordinateCto FromLatLng(decimal? latitude, decimal? longitude)
        {
            if (!latitude.HasValue || !longitude.HasValue)
                return null;
            return new CoordinateCto
            {
                Latitude = latitude.Value,
                Longitude = longitude.Value
            };
        }
    }
}