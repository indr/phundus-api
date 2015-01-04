namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System.Runtime.Serialization;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class ReservationCancelled : DomainEvent
    {
        public ReservationCancelled(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId, Period period, int quantity)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");
            AssertionConcern.AssertArgumentNotNull(period, "Period must be provided.");
            AssertionConcern.AssertArgumentGreaterThanZero(quantity, "Quantity must be greater than zero.");

            ReservationId = reservationId.Id;
        }

        protected ReservationCancelled()
        {
        }

        public string ReservationId { get; protected set; }
    }
}