namespace Phundus.Inventory.AvailabilityAndReservation.Repositories
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using Model;

    public interface IReservationRepository
    {
        IEnumerable<Reservation> Find(int articleId, Guid orderItemToExclude);
        IEnumerable<Reservation> Find(ArticleId articleId, Guid orderItemToExclude);
    }
}