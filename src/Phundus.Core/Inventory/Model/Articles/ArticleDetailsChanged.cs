namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class ArticleDetailsChanged : DomainEvent
    {
        public ArticleDetailsChanged(Manager initiator, ArticleShortId articleShortId, ArticleId articleId,
            OwnerId ownerId, string name, string brand, string color)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");

            Initiator = initiator.ToActor();
            ArticleShortId = articleShortId.Id;
            ArticleId = articleId.Id;
            OwnerId = ownerId.Id;
            Name = name;
            Brand = brand;
            Color = color;
        }

        protected ArticleDetailsChanged()
        {
        }

        [DataMember(Order = 1)]
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleShortId { get; protected set; }

        [DataMember(Order = 3)]
        public Guid ArticleId { get; protected set; }

        [DataMember(Order = 4)]
        public Guid OwnerId { get; protected set; }

        [DataMember(Order = 5)]
        public string Name { get; protected set; }

        [DataMember(Order = 6)]
        public string Brand { get; protected set; }

        [DataMember(Order = 7)]
        public string Color { get; protected set; }
    }
}