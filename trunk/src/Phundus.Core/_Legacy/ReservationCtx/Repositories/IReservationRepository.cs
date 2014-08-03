namespace Phundus.Core.ReservationCtx.Repositories
{
    using System.Collections.Generic;
    using Inventory.Model;
    using Model;

    public interface IReservationRepository
    {
        ICollection<Reservation> Find(Article article);
    }
}