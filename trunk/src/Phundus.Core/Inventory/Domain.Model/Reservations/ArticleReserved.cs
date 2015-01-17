namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using System.Runtime.Serialization;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;
    using Shop.Domain.Model.Ordering;

    [DataContract]
    public class ArticleReserved : DomainEvent
    {
        public ArticleReserved(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId,
            OrderId orderId, Period period, int quantity, ReservationStatus reservationStatus)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(orderId, "Order id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");
            AssertionConcern.AssertArgumentGreaterThanZero(quantity, "Quantity must be greater than zero.");

            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            ReservationId = reservationId.Id;
            OrderId = orderId.Id;
            Period = period;
            Quantity = quantity;
            ReservationStatus = reservationStatus.ToString();
        }

        protected ArticleReserved()
        {
        }

        public Period Period
        {
            get { return new Period(FromUtc, ToUtc); }
            set
            {
                FromUtc = value.FromUtc;
                ToUtc = value.ToUtc;
            }
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
        public int Quantity { get; protected set; }

        [DataMember(Order = 9)]
        public string ReservationStatus { get; protected set; }
    }
}