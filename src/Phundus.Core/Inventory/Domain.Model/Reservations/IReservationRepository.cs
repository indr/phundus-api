namespace Phundus.Core.Inventory.Domain.Model.Reservations
{
    using Catalog;
    using IdentityAndAccess.Domain.Model.Organizations;

    public interface IReservationRepository
    {
        Reservation Get(OrganizationId organizationId, ArticleId articleId, ReservationId reservationId);

        ReservationId GetNextIdentity();

        void Save(Reservation reservation);
    }
}