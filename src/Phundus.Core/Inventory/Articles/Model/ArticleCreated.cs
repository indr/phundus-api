namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class ArticleCreated : DomainEvent
    {
        public ArticleCreated(Initiator initiator, Owner owner, StoreId storeId, string storeName, ArticleShortId articleShortId,
            ArticleId articleId, string name, int grossStock, decimal publicPrice, decimal? memberPrice)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (owner == null) throw new ArgumentNullException("owner");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (name == null) throw new ArgumentNullException("name");

            Initiator = initiator;
            Owner = owner;
            StoreName = storeName;
            StoreId = storeId.Id;
            ArticleShortId = articleShortId.Id;
            ArticleId = articleId.Id;
            Name = name;
            GrossStock = grossStock;
            PublicPrice = publicPrice;
            MemberPrice = memberPrice;
        }

        protected ArticleCreated()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Owner Owner { get; protected set; }

        [DataMember(Order = 3)]
        public Guid StoreId { get; protected set; }

        [DataMember(Order = 10)]
        public string StoreName { get; protected set; }

        [DataMember(Order = 4)]
        public int ArticleShortId { get; protected set; }

        [DataMember(Order = 5)]
        public Guid ArticleId { get; protected set; }

        [DataMember(Order = 6)]
        public string Name { get; set; }

        [DataMember(Order = 7)]
        public int GrossStock { get; set; }

        [DataMember(Order = 8)]
        public decimal PublicPrice { get; protected set; }

        [DataMember(Order = 9)]
        public decimal? MemberPrice { get; protected set; }
    }
}