namespace Phundus.Core.ReservationCtx.Repositories
{
    using System.Collections.Generic;
    using Model;

    public interface IReservationRepository
    {
        IEnumerable<Reservation> Find(int articleId);
    }
}