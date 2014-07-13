namespace phiNdus.fundus.Domain.Repositories
{
    using System.Collections.Generic;
    using Entities;

    public interface IReservationRepository
    {
        ICollection<Reservation> Find(Article article);
    }
}