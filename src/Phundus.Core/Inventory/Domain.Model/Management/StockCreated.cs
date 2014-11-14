namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Runtime.Serialization;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class StockCreated : DomainEvent
    {
        protected StockCreated()
        {
        }

        public StockCreated(OrganizationId organizationId, ArticleId articleId, StockId stockId)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");

            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            StockId = stockId.Id;
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; set; }

        [DataMember(Order = 3)]
        public string StockId { get; set; }
    }
}