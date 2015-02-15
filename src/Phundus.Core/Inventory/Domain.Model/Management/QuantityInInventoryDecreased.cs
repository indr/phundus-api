namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Runtime.Serialization;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    /// <summary>
    /// Sub-Events: Material verkauft, Material ausgemustert
    /// </summary>
    [DataContract]
    public class QuantityInInventoryDecreased : DomainEvent
    {
        protected QuantityInInventoryDecreased()
        {
        }

        public QuantityInInventoryDecreased(OrganizationId organizationId, ArticleId articleId, StockId stockId,
            int change, int total, DateTime asOfUtc, string comment)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");
            AssertionConcern.AssertArgumentNotZero(change, "Change must be greater or less than zero.");
            AssertionConcern.AssertArgumentNotEmpty(asOfUtc, "As of utc must be provided.");

            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            StockId = stockId.Id;
            Change = change;
            Total = total;
            AsOfUtc = asOfUtc;
            Comment = comment;
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; set; }

        [DataMember(Order = 3)]
        public string StockId { get; set; }

        [DataMember(Order = 4)]
        public int Change { get; set; }

        [DataMember(Order = 5)]
        public int Total { get; set; }

        [DataMember(Order = 6)]
        public DateTime AsOfUtc { get; set; }

        [DataMember(Order = 7)]
        public string Comment { get; set; }
    }
}