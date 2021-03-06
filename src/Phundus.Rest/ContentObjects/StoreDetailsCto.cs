namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class StoreDetailsCto
    {
        [JsonProperty("storeId")]
        public Guid StoreId { get; set; }

        [JsonProperty("ownerId")]
        public Guid OwnerId { get; set; }

        [JsonProperty("ownerType")]
        public string OwnerType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contact")]
        public ContactCto Contact { get; set; }

        [JsonProperty("coordinate")]
        public CoordinateCto Coordinate { get; set; }

        [JsonProperty("openingHours")]
        public string OpeningHours { get; set; }
    }
}