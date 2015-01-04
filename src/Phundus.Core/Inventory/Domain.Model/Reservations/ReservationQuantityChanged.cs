namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using System.Runtime.Serialization;
    using Catalog;
    using Common;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    [DataContract]
    public class ReservationQuantityChanged : DomainEvent
    {
        public ReservationQuantityChanged(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId,
            int oldQuantity, int newQuantity)
        {
            AssertionConcern.AssertArgumentNotNull(organizationId, "Organization id must be provided.");
            AssertionConcern.AssertArgumentNotNull(articleId, "Article id must be provided.");
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");
            AssertionConcern.AssertArgumentGreaterThan(oldQuantity, 0, "Old quantity must be greater than zero.");
            AssertionConcern.AssertArgumentGreaterThan(newQuantity, 0, "New quantity must be greater than zero.");

            OrganizationId = organizationId.Id;
            ArticleId = articleId.Id;
            ReservationId = reservationId.Id;
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
        }

        protected ReservationQuantityChanged()
        {
        }

        [DataMember(Order = 1)]
        public int OrganizationId { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleId { get; protected set; }

        [DataMember(Order = 3)]
        public string ReservationId { get; protected set; }

        [DataMember(Order = 4)]
        public int OldQuantity { get; protected set; }

        [DataMember(Order = 5)]
        public int NewQuantity { get; protected set; }
    }
}