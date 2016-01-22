namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class ArticleCreated : DomainEvent
    {
        public ArticleCreated(Initiator initiator, Owner owner, StoreId storeId, int articleId,
            Guid articleGuid, string name, int grossStock, decimal memberPrice, decimal publicPrice)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (owner == null) throw new ArgumentNullException("owner");
            if (storeId == null) throw new ArgumentNullException("storeId");
            if (name == null) throw new ArgumentNullException("name");
            Initiator = initiator;
            Owner = owner;
            StoreId = storeId.Id;
            ArticleId = articleId;
            ArticleGuid = articleGuid;
            Name = name;
            GrossStock = grossStock;
            MemberPrice = memberPrice;
            PublicPrice = publicPrice;
        }

        [Obsolete]
        public ArticleCreated() { }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public Owner Owner { get; protected set; }

        [DataMember(Order = 3)]
        public Guid StoreId { get; protected set; }

        [DataMember(Order = 4)]
        public int ArticleId { get; protected set; }

        [DataMember(Order = 5)]
        public Guid ArticleGuid { get; protected set; }

        [DataMember(Order = 6)]
        public string Name { get; set; }

        [DataMember(Order = 7)]
        public int GrossStock { get; set; }

        [DataMember(Order = 8)]
        public decimal MemberPrice { get; protected set; }

        [DataMember(Order = 9)]
        public decimal PublicPrice { get; protected set; }
    }
}