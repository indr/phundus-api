namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using System.Runtime.Serialization;
    using Catalog;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Shop.Domain.Model.Ordering;

    [DataContract]
    public class ArticleReserved : DomainEvent
    {
        public ArticleReserved(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId,
            OrderId orderId, Period period, int amount)
        {
            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            ReservationId = reservationId.Id;
            OrderId = orderId.Id;
            FromUtc = period.FromUtc;
            ToUtc = period.ToUtc;
            Amount = amount;
        }

        protected ArticleReserved()
        {
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; protected set; }

        [DataMember(Order = 3)]
        public string ReservationId { get; protected set; }

        [DataMember(Order = 4)]
        public int OrderId { get; protected set; }

        [DataMember(Order = 6)]
        public DateTime FromUtc { get; protected set; }

        [DataMember(Order = 7)]
        public DateTime ToUtc { get; protected set; }

        [DataMember(Order = 8)]
        public int Amount { get; protected set; }
    }
}