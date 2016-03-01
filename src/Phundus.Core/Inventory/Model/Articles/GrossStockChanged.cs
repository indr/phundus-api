namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class GrossStockChanged : DomainEvent
    {
        public GrossStockChanged(Initiator initiator, ArticleShortId articleShortId, ArticleId articleId,
            OwnerId ownerId, int oldGrossStock, int newGrossStock)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");

            Initiator = initiator;
            ArticleShortId = articleShortId.Id;
            ArticleId = articleId.Id;
            OwnerId = ownerId.Id;
            OldGrossStock = oldGrossStock;
            NewGrossStock = newGrossStock;
        }

        protected GrossStockChanged()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleShortId { get; protected set; }

        [DataMember(Order = 3)]
        public Guid ArticleId { get; protected set; }

        [DataMember(Order = 4)]
        public Guid OwnerId { get; protected set; }

        [DataMember(Order = 5)]
        public int OldGrossStock { get; protected set; }

        [DataMember(Order = 6)]
        public int NewGrossStock { get; protected set; }
    }
}