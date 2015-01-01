﻿namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using System.Runtime.Serialization;
    using Catalog;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class ReservationTimeRangeChanged : DomainEvent
    {
        public ReservationTimeRangeChanged(OrganizationId organizationId, ArticleId articleId,
            ReservationId reservationId, Period period)
        {
            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            ReservationId = reservationId.Id;
            FromUtc = period.FromUtc;
            ToUtc = period.ToUtc;
        }

        protected ReservationTimeRangeChanged()
        {
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; protected set; }

        [DataMember(Order = 3)]
        public string ReservationId { get; protected set; }

        [DataMember(Order = 4)]
        public DateTime FromUtc { get; protected set; }

        [DataMember(Order = 5)]
        public DateTime ToUtc { get; protected set; }
    }
}