namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Runtime.Serialization;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class AllocationPeriodChanged : DomainEvent
    {
        public AllocationPeriodChanged(OrganizationId organizationId, ArticleId articleId, StockId stockId,
            AllocationId allocationId, Period oldPeriod, Period newPeriod)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");
            AssertionConcern.AssertArgumentNotNull(allocationId, "Allocation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(oldPeriod, "Old period must be provided.");
            AssertionConcern.AssertArgumentNotNull(newPeriod, "New period must be provided.");

            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            StockId = stockId.Id;
            AllocationId = allocationId.Id;
            OldPeriod = oldPeriod;
            NewPeriod = newPeriod;
        }

        protected AllocationPeriodChanged()
        {
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; protected set; }

        [DataMember(Order = 3)]
        public string StockId { get; protected set; }

        [DataMember(Order = 4)]
        public Guid AllocationId { get; protected set; }

        [DataMember(Order = 5)]
        public DateTime OldFromUtc { get; protected set; }

        [DataMember(Order = 6)]
        public DateTime OldToUtc { get; protected set; }

        [DataMember(Order = 7)]
        public DateTime NewFromUtc { get; protected set; }

        [DataMember(Order = 8)]
        public DateTime NewToUtc { get; protected set; }

        public Period OldPeriod
        {
            get { return new Period(OldFromUtc, OldToUtc); }
            private set
            {
                OldFromUtc = value.FromUtc;
                OldToUtc = value.ToUtc;
            }
        }

        public Period NewPeriod
        {
            get { return new Period(NewFromUtc, NewToUtc); }
            private set
            {
                NewFromUtc = value.FromUtc;
                NewToUtc = value.ToUtc;
            }
        }
    }
}