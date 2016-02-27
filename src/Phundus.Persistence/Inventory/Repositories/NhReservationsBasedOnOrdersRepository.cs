namespace Phundus.Persistence.Inventory.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate;
    using NHibernate.Linq;
    using Phundus.Inventory.AvailabilityAndReservation.Model;
    using Phundus.Inventory.AvailabilityAndReservation.Repositories;
    using Phundus.Shop.Orders.Model;

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
                        where oi.ArticleShortId.Id == articleId
                         && oi.ItemId.Id != orderItemToExclude
                        select oi;

            result.AddRange(factory.Create(query));

            return result;
        }
    }
}