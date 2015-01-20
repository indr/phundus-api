﻿namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System;
    using System.Runtime.Serialization;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class AllocationDiscarded : DomainEvent
    {
        public AllocationDiscarded(OrganizationId organizationId, ArticleId articleId, StockId stockId,
            AllocationId allocationId, Period period, int quantity)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(stockId, "Stock id must be provided.");
            AssertionConcern.AssertArgumentNotNull(allocationId, "Allocation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");

            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            StockId = stockId.Id;
            AllocationId = allocationId.Id;
            Period = period;
            Quantity = quantity;
        }

        protected AllocationDiscarded()
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
        public DateTime FromUtc { get; protected set; }

        [DataMember(Order = 6)]
        public DateTime ToUtc { get; protected set; }

        [DataMember(Order = 7)]
        public int Quantity { get; protected set; }

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