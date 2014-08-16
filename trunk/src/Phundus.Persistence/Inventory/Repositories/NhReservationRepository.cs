namespace Phundus.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Inventory.AvailabilityAndReservation.Model;
    using Core.ReservationCtx.Repositories;
    using Core.ReservationCtx.Services;
    using Core.Shop.Orders.Model;
    using NHibernate;
    using NHibernate.Linq;

    public class NhReservationRepository : IReservationRepository
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
                        where oi.Article.Id == articleId
                        //&& oi.To > DateTime.Today
                        select oi;

            result.AddRange(factory.Create(query));

            return result;
        }
    }
}