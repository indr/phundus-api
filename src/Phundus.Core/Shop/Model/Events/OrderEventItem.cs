namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderEventItem
    {
        public OrderEventItem(Guid itemId, ArticleId articleId, ArticleShortId articleShortId, string text,
            decimal unitPricePerWeek, DateTime fromUtc, DateTime toUtc, int quantity, decimal itemTotal)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (text == null) throw new ArgumentNullException("text");
            ItemId = itemId;
            ArticleId = articleId.Id;
            ArticleShortId = articleShortId.Id;
            Text = text;
            UnitPricePerWeek = unitPricePerWeek;
            FromUtc = fromUtc;
            ToUtc = toUtc;
            Quantity = quantity;
            ItemTotal = itemTotal;
        }

        protected OrderEventItem()
        {
        }

        [DataMember(Order = 1)]
        public Guid ItemId { get; set; }

        [DataMember(Order = 2)]
        public Guid ArticleId { get; set; }

        [DataMember(Order = 3)]
        public int ArticleShortId { get; set; }

        [DataMember(Order = 4)]
        public string Text { get; set; }

        [DataMember(Order = 5)]
        public decimal UnitPricePerWeek { get; set; }

        [DataMember(Order = 6)]
        public DateTime FromUtc { get; set; }

        [DataMember(Order = 7)]
        public DateTime ToUtc { get; set; }

        [DataMember(Order = 8)]
        public int Quantity { get; set; }

        [DataMember(Order = 9)]
        public decimal ItemTotal { get; set; }
    }
}