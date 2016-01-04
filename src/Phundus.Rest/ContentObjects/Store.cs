namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class Store
    {
        [JsonProperty("storeId")]
        public Guid StoreId { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("coordinate")]
        public Coordinate Coordinate { get; set; }

        [JsonProperty("openingHours")]
        public string OpeningHours { get; set; }
    }
}