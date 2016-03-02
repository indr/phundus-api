namespace Phundus.Inventory.Model.Reservations
{
    using System.Collections.Generic;
    using AvailabilityAndReservation.Model;
    using Common.Domain.Model;

    public interface IReservationRepository
    {
        IEnumerable<Reservation> Find(ArticleId articleId, OrderLineId orderLineIdToExclude);
    }
}