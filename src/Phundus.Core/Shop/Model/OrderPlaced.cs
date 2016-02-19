namespace Phundus.Shop.Orders.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Shop.Model;

    [DataContract]
    public class OrderPlaced : DomainEvent
    {
        public OrderPlaced(Initiator initiator, OrderId orderId, ShortOrderId shortOrderId, Lessor lessor, Lessee lessee,
            int status, decimal totalPrice, IList<OrderPlaced.Item> items)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (orderId == null) throw new ArgumentNullException("orderId");
            if (shortOrderId == null) throw new ArgumentNullException("shortOrderId");
            if (lessor == null) throw new ArgumentNullException("lessor");
            if (lessee == null) throw new ArgumentNullException("lessee");
            if (items == null) throw new ArgumentNullException("items");
            Initiator = initiator;
            OrderId = orderId.Id;
            ShortOrderId = shortOrderId.Id;
            Lessor = lessor;
            Lessee = lessee;
            Status = status;
            TotalPrice = totalPrice;
            LessorId = lessor.LessorId.Id;
            Items = items;
        }

        protected OrderPlaced()
        {
        }

        [DataMember(Order = 1)]
        public int ShortOrderId { get; protected set; }

        [DataMember(Order = 2)]
        public Guid LessorId { get; set; }

        [DataMember(Order = 3)]
        public Guid OrderId { get; set; }

        [DataMember(Order = 4)]
        public Initiator Initiator { get; set; }

        [DataMember(Order = 5)]
        public Lessor Lessor { get; set; }

        [DataMember(Order = 6)]
        public Lessee Lessee { get; set; }

        [DataMember(Order = 7)]
        public int Status { get; set; }

        [DataMember(Order = 8)]
        public decimal TotalPrice { get; set; }

        [DataMember(Order = 9)]
        public IList<Item> Items { get; set; }

        [DataContract]
        public class Item
        {
            public Item(Guid itemId, ArticleId articleId, ArticleShortId articleShortId, string text,
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

            protected Item()
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
}