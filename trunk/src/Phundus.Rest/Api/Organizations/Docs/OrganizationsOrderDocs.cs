namespace Phundus.Rest.Api.Organizations
{
    using System;
    using Newtonsoft.Json;

    public class OrdersPatchRequestContent
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class OrdersPostRequestContent
    {
        [JsonProperty("ownerId")]
        public Guid OwnerId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class OrdersItemPostRequestContent
    {
        public int ArticleId { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public int Amount { get; set; }
    }

    public class OrderItemPatchDoc
    {
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }
        public int Amount { get; set; }
        public decimal ItemTotal { get; set; }
    }
}