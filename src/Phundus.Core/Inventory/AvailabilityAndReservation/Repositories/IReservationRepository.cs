namespace Phundus.Core.ReservationCtx.Repositories
{
    using System.Collections.Generic;
    using Inventory.AvailabilityAndReservation.Model;

    public interface IReservationRepository
    {
        IEnumerable<Reservation> Find(int articleId);
    }
}