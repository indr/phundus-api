﻿namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using System.Runtime.Serialization;
    using Articles;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class ReservationCreated : DomainEvent
    {
        public ReservationCreated(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId,
            TimeRange timeRange, int amount)
        {
            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            ReservationId = reservationId.Id;
            FromUtc = timeRange.FromUtc;
            ToUtc = timeRange.ToUtc;
        }

        protected ReservationCreated()
        {
        }

        [DataMember(Order = 1)]
        public string OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public string ArticleId { get; protected set; }

        [DataMember(Order = 3)]
        public string ReservationId { get; protected set; }

        [DataMember(Order = 4)]
        public DateTime FromUtc { get; protected set; }

        [DataMember(Order = 5)]
        public DateTime ToUtc { get; protected set; }

        [DataMember(Order = 6)]
        public int Amount { get; protected set; }
    }
}