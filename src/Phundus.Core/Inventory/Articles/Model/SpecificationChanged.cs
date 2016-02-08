namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class SpecificationChanged : DomainEvent
    {
        public SpecificationChanged(Initiator initiator, ArticleId articleIntegralId, ArticleGuid articleGuid,
            OwnerId ownerId, string specification)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleIntegralId == null) throw new ArgumentNullException("articleIntegralId");
            if (articleGuid == null) throw new ArgumentNullException("articleGuid");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (specification == null) throw new ArgumentNullException("specification");
            Initiator = initiator;
            ArticleIntegralId = articleIntegralId.Id;
            ArticleGuid = articleGuid.Id;
            OwnerId = ownerId.Id;
            Specification = specification;
        }

        [Obsolete("protected")]
        public SpecificationChanged()
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
        public string Specification { get; set; }
    }
}