namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class OrderEventLine : ValueObject
    {
        public OrderEventLine(OrderLineId orderLineId, ArticleId articleId, ArticleShortId articleShortId, string text,
            decimal unitPricePerWeek, Period period, int quantity, decimal lineTotal)
        {
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (text == null) throw new ArgumentNullException("text");
            if (period == null) throw new ArgumentNullException("period");
            ItemId = orderLineId.Id;
            ArticleId = articleId.Id;
            ArticleShortId = articleShortId.Id;
            Text = text;
            UnitPricePerWeek = unitPricePerWeek;
            FromUtc = period.FromUtc;
            ToUtc = period.ToUtc;
            Quantity = quantity;
            LineTotal = lineTotal;
        }

        protected OrderEventLine()
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
        public decimal LineTotal { get; set; }

        public Period Period
        {
            get { return new Period(FromUtc, ToUtc); }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ItemId;
        }
    }
}