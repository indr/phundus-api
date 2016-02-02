namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class ArticleDeleted : DomainEvent
    {
        public ArticleDeleted(Initiator initiator, ArticleId articleIntegralId, ArticleGuid articleGuid, OwnerId ownerId)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleIntegralId == null) throw new ArgumentNullException("articleIntegralId");
            if (articleGuid == null) throw new ArgumentNullException("articleGuid");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            Initiator = initiator;
            ArticleIntegralId = articleIntegralId.Id;
            ArticleGuid = articleGuid.Id;
            OwnerId = ownerId.Id;
        }

        protected ArticleDeleted()
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
    }
}