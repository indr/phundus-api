namespace Phundus.Rest.ContentObjects
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class OrderDetail : Order
    {
        private IList<OrderItem> _items = new List<OrderItem>();

        [JsonProperty("items")]
        public IList<OrderItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        [JsonProperty("totalPrice")]
        public decimal TotalPrice { get; set; }
    }
}