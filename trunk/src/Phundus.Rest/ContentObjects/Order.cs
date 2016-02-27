namespace Phundus.Rest.ContentObjects
{
    using System;
    using Newtonsoft.Json;

    public class Order
    {
        [JsonProperty("orderId")]
        public Guid OrderId { get; set; }

        [JsonProperty("orderShortId")]
        public int OrderShortId { get; set; }

        [JsonProperty("createdAtUtc")]
        public DateTime CreatedAtUtc { get; set; }

        [JsonProperty("modifiedAtUtc")]
        public DateTime? ModifiedAtUtc { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("lessorId")]
        public Guid LessorId { get; set; }

        [JsonProperty("lessorName")]
        public string LessorName { get; set; }

        [JsonProperty("lessee")]
        public Lessee Lessee { get; set; }
    }
}