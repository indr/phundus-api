namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System;
    using System.Runtime.Serialization;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class ReservationCancelled : DomainEvent
    {
        public ReservationCancelled(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId,
            Period period, int quantity)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");
            AssertionConcern.AssertArgumentGreaterThanZero(quantity, "Quantity must be greater than zero.");

            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            ReservationId = reservationId.Id;
            Period = period;
            Quantity = quantity;
        }

        protected ReservationCancelled()
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
        public DateTime FromUtc { get; protected set; }

        [DataMember(Order = 5)]
        public DateTime ToUtc { get; protected set; }

        [DataMember(Order = 6)]
        public int Quantity { get; protected set; }
    }
}