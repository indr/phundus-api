namespace Phundus.Inventory.AvailabilityAndReservation.Repositories
{
    using System;
    using System.Collections.Generic;
    using Model;

    public interface IReservationRepository
    {
        IEnumerable<Reservation> Find(int articleId, Guid orderItemToExclude);
    }
}