namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class ArticleDeleted : DomainEvent
    {
        public ArticleDeleted(Initiator initiator, ArticleShortId articleShortId, ArticleId articleId, OwnerId ownerId)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            Initiator = initiator;
            ArticleShortId = articleShortId.Id;
            ArticleId = articleId.Id;
            OwnerId = ownerId.Id;
        }

        protected ArticleDeleted()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; set; }

        [DataMember(Order = 2)]
        public int ArticleShortId { get; set; }

        [DataMember(Order = 3)]
        public Guid ArticleId { get; set; }

        [DataMember(Order = 4)]
        public Guid OwnerId { get; set; }
    }
}