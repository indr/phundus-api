namespace Phundus.Core.Shop.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CartDto
    {
        private IList<CartItemDto> _items = new List<CartItemDto>();

        public int Id { get; set; }
        public int Version { get; set; }

        public IList<CartItemDto> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public decimal TotalPrice
        {
            get { return Items.Sum(s => s.LineTotal); }
        }

        public int CustomerId { get; set; }

        public bool AreItemsAvailable { get; set; }
    }

    public class CartItemDto
    {
        public int Id { get; set; }
        public int Version { get; set; }

        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public string Text { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }

        public bool IsAvailable { get; set; }

        public string OrganizationName { get; set; }
    }
}