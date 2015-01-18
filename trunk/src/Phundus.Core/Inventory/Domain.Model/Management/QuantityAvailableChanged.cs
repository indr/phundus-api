namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Data.Linq.Mapping;
    using System.Runtime.Serialization;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class QuantityAvailableChanged : DomainEvent
    {
        public QuantityAvailableChanged(OrganizationId organizationId, ArticleId articleId, StockId stockId, Period period, int change)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided");
            AssertionConcern.AssertArgumentNotZero(change, "Change must be greater or less than zero.");

            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            StockId = stockId.Id;
            Period = period;
            Change = change;
        }

        protected QuantityAvailableChanged()
        {
            
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; set; }

        [DataMember(Order = 3)]
        public string StockId { get; set; }

        [DataMember(Order = 4)]
        public DateTime FromUtc { get; protected set; }

        [DataMember(Order = 5)]
        public DateTime ToUtc { get; protected set; }

        [DataMember(Order = 6)]
        public int Change { get; set; }

        public Period Period
        {
            get { return new Period(FromUtc, ToUtc);}
            set
            {
                FromUtc = value.FromUtc;
                ToUtc = value.ToUtc;
            }
        }
    }
}