namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class SpecificationChanged : DomainEvent
    {
        public SpecificationChanged(Manager initiator, ArticleShortId articleShortId, ArticleId articleId,
            OwnerId ownerId, string specification)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (specification == null) throw new ArgumentNullException("specification");

            Initiator = initiator.ToActor();
            ArticleShortId = articleShortId.Id;
            ArticleId = articleId.Id;
            OwnerId = ownerId.Id;
            Specification = specification;
        }

        protected SpecificationChanged()
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
        public string Specification { get; protected set; }
    }
}