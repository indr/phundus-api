namespace Phundus.Inventory.Model.Articles
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class ProductTagged : DomainEvent
    {
        public ProductTagged(Manager manager, ArticleId articleId, OwnerId ownerId, string tagName)
        {
            Actor = manager.ToActor();
            ArticleId = articleId.Id;
            OwnerId = ownerId.Id;
            TagName = tagName;
        }

        protected ProductTagged()
        {
        }
        
        [DataMember(Order = 1)]
        public Actor Actor { get; protected set; }

        [DataMember(Order = 2)]
        public Guid ArticleId { get; protected set; }

        [DataMember(Order = 3)]
        public Guid OwnerId { get; protected set; }

        [DataMember(Order = 4)]
        public string TagName { get; protected set; }
    }
}