namespace Phundus.Core.Repositories
{
    using System.Collections.Generic;
    using Phundus.Core.Entities;

    public interface IReservationRepository
    {
        ICollection<Reservation> Find(Article article);
    }
}