namespace Phundus.Specs.ContentTypes
{
    using System;
    using Newtonsoft.Json;

    public class StoresPostRequestContent
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
    }

    public class StoresPostOkResponseContent
    {
        [JsonProperty("storeId")]
        public Guid StoreId { get; set; }
    }

    public class Store
    {
        [JsonProperty("storeId")]
        public Guid StoreId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("coordinate")]
        public Coordinate Coordinate { get; set; }

        [JsonProperty("openingHours")]
        public string OpeningHours { get; set; }
    }

    public class Coordinate
    {
        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }
    }
}