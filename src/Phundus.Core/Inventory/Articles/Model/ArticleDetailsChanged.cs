namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class ArticleDetailsChanged : DomainEvent
    {
        public ArticleDetailsChanged(Initiator initiator, ArticleId articleIntegralId, ArticleGuid articleGuid,
            OwnerId ownerId, string name, string brand, string color)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleIntegralId == null) throw new ArgumentNullException("articleIntegralId");
            if (articleGuid == null) throw new ArgumentNullException("articleGuid");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            Initiator = initiator;
            ArticleIntegralId = articleIntegralId.Id;
            ArticleGuid = articleGuid.Id;
            OwnerId = ownerId.Id;
            Name = name;
            Brand = brand;
            Color = color;
        }

        protected ArticleDetailsChanged()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleIntegralId { get; set; }

        [DataMember(Order = 3)]
        public Guid ArticleGuid { get; set; }

        [DataMember(Order = 4)]
        public Guid OwnerId { get; set; }

        [DataMember(Order = 5)]
        public string Name { get; set; }

        [DataMember(Order = 6)]
        public string Brand { get; set; }

        [DataMember(Order = 7)]
        public string Color { get; set; }
    }
}