namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class PricesChanged : DomainEvent
    {
        public PricesChanged(Initiator initiator, ArticleShortId articleShortId, ArticleId articleId, OwnerId ownerId,
            decimal publicPrice, decimal? memberPrice)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");

            Initiator = initiator;
            ArticleShortId = articleShortId.Id;
            ArticleId = articleId.Id;
            OwnerId = ownerId.Id;
            PublicPrice = publicPrice;
            MemberPrice = memberPrice;
        }

        protected PricesChanged()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleShortId { get; protected set; }

        [DataMember(Order = 3)]
        public Guid ArticleId { get; protected set; }

        [DataMember(Order = 4)]
        public decimal PublicPrice { get; protected set; }

        [DataMember(Order = 5)]
        public decimal? MemberPrice { get; protected set; }

        [DataMember(Order = 6)]
        public Guid OwnerId { get; protected set; }
    }
}