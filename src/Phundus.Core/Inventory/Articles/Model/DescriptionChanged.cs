namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class DescriptionChanged : DomainEvent
    {
        public DescriptionChanged(Initiator initiator, ArticleShortId articleShortId, ArticleId articleId, OwnerId ownerId, string description)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (description == null) throw new ArgumentNullException("description");
            Initiator = initiator;
            ArticleShortId = articleShortId.Id;
            ArticleId = articleId.Id;
            OwnerId = ownerId.Id;
            Description = description;
        }

        protected DescriptionChanged()
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

        [DataMember(Order = 5)]
        public string Description { get; set; }
    }
}