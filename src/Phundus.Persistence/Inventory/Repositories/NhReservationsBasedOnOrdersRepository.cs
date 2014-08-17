namespace Phundus.Persistence.Inventory.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Inventory.AvailabilityAndReservation.Model;
    using Core.ReservationCtx.Repositories;
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

        public IEnumerable<Reservation> Find(int articleId)
        {
            var factory = new ReservationFactory();
            var result = new List<Reservation>();

            var query = from oi in OrderItems
                        where oi.ArticleId == articleId
                        //&& oi.To > DateTime.Today
                        select oi;

            result.AddRange(factory.Create(query));

            return result;
        }
    }
}