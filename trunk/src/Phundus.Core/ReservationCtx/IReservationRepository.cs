namespace Phundus.Core.ReservationCtx
{
    using System.Collections.Generic;
    using InventoryCtx;

    public interface IReservationRepository
    {
        ICollection<Reservation> Find(Article article);
    }
}