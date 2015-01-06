namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class CancelReservation : ICommand
    {
        public CancelReservation(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId)
        {
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");

            OrganizationId = organizationId;
            ArticleId = articleId;
            ReservationId = reservationId;
        }

        public OrganizationId OrganizationId { get; private set; }
        public ArticleId ArticleId { get; private set; }
        public ReservationId ReservationId { get; private set; }
    }
}