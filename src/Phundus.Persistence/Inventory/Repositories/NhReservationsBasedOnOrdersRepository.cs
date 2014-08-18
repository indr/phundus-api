namespace Phundus.Persistence.Inventory.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Inventory.AvailabilityAndReservation.Model;
    using Core.Inventory.AvailabilityAndReservation.Repositories;
    using Core.Shop.Orders.Model;
    using NHibernate;
    using NHibernate.Linq;

    public class NhReservationsBasedOnOrdersRepository : IReservationRepository
    {
        public Func<ISession> Session { get; set; }

        private IQueryable<OrderItem> OrderItems
        {
            get { return Session().Query<OrderItem>(); }
        }

        public IEnumerable<Reservation> Find(int articleId, Guid orderItemToExclude)
        {
            var factory = new ReservationFactory();
            var result = new List<Reservation>();

            var query = from oi in OrderItems
                        where oi.ArticleId == articleId
                         && oi.Id != orderItemToExclude
                        select oi;

            result.AddRange(factory.Create(query));

            return result;
        }
    }
}