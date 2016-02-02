namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class PricesChanged : DomainEvent
    {
        public PricesChanged(Initiator initiator, int articleIntegralId, ArticleGuid articleGuid,
            decimal publicPrice, decimal? memberPrice)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            Initiator = initiator;
            ArticleIntegralId = articleIntegralId;
            ArticleGuid = articleGuid.Id;
            PublicPrice = publicPrice;
            MemberPrice = memberPrice;
        }

        protected PricesChanged()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleIntegralId { get; set; }

        [DataMember(Order = 3)]
        public Guid ArticleGuid { get; set; }

        [DataMember(Order = 4)]
        public decimal PublicPrice { get; set; }

        [DataMember(Order = 5)]
        public decimal? MemberPrice { get; set; }
    }
}