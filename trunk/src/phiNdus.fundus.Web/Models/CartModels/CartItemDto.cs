namespace Phundus.Web.Models.CartModels
{
    using System;

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