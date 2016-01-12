namespace Phundus.Specs.ContentTypes
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class UsersCartGetOkResponseContent
    {
        [JsonProperty("items")]
        public List<CartItems> Items { get; set; }
    }

    public class CartItems
    {
    }

    public class UserCartItemsPostOkResponseContent
    {
        [JsonProperty("cartItemId")]
        public Guid CartItemId { get; set; }
    }
}