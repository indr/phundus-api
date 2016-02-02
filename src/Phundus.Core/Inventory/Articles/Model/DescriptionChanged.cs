namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class DescriptionChanged : DomainEvent
    {
        public DescriptionChanged(Initiator initiator, ArticleId articleId, ArticleGuid articleGuid, OwnerId ownerId, string description)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (articleGuid == null) throw new ArgumentNullException("articleGuid");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (description == null) throw new ArgumentNullException("description");
            Initiator = initiator;
            ArticleIntegralId = articleId.Id;
            ArticleGuid = articleGuid.Id;
            OwnerId = ownerId.Id;
            Description = description;
        }

        protected DescriptionChanged()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; set; }

        [DataMember(Order = 2)]
        public int ArticleIntegralId { get; set; }

        [DataMember(Order = 3)]
        public Guid ArticleGuid { get; set; }

        [DataMember(Order = 4)]
        public Guid OwnerId { get; set; }

        [DataMember(Order = 5)]
        public string Description { get; set; }
    }
}