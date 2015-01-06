namespace Phundus.Core.Inventory.Application.Commands
{
    using Common;
    using Common.Cqrs;
    using Cqrs;
    using Domain.Model.Catalog;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class ChangeReservationQuantity : ICommand
    {
        public ChangeReservationQuantity(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId, int quantity)
        {
            AssertionConcern.AssertArgumentNotNull(reservationId, "Reservation id must be provided.");

            OrganizationId = organizationId;
            ArticleId = articleId;
            ReservationId = reservationId;
            Quantity = quantity;
        }

        public OrganizationId OrganizationId { get; private set; }
        public ArticleId ArticleId { get; private set; }
        public ReservationId ReservationId { get; private set; }
        public int Quantity { get; private set; }
    }

    public class ChangeReservationQuantityHandler : IHandleCommand<ChangeReservationQuantity>
    {
        public IReservationRepository ReservationRepository { get; set; }

        public void Handle(ChangeReservationQuantity command)
        {
            var reservation = ReservationRepository.Get(command.OrganizationId, command.ArticleId, command.ReservationId);
            
            reservation.ChangeQuantity(command.Quantity);

            ReservationRepository.Save(reservation);
        }
    }
}