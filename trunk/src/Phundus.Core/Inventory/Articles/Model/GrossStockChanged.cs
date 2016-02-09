﻿namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class GrossStockChanged : DomainEvent
    {
        public GrossStockChanged(Initiator initiator, ArticleId articleIntegralId, ArticleGuid articleGuid,
            OwnerId ownerId, int oldGrossStock, int newGrossStock)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleIntegralId == null) throw new ArgumentNullException("articleIntegralId");
            if (articleGuid == null) throw new ArgumentNullException("articleGuid");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            Initiator = initiator;
            ArticleIntegralId = articleIntegralId.Id;
            ArticleGuid = articleGuid.Id;
            OwnerId = ownerId.Id;
            OldGrossStock = oldGrossStock;
            NewGrossStock = newGrossStock;
        }

        protected GrossStockChanged()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleIntegralId { get; set; }

        [DataMember(Order = 3)]
        public Guid ArticleGuid { get; set; }

        [DataMember(Order = 4)]
        public Guid OwnerId { get; set; }

        [DataMember(Order = 5)]
        public int OldGrossStock { get; set; }

        [DataMember(Order = 6)]
        public int NewGrossStock { get; set; }
    }
}