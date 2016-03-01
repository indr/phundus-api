namespace Phundus.Persistence.Inventory.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Domain.Model;
    using NHibernate;
    using NHibernate.Linq;
    using Phundus.Inventory.AvailabilityAndReservation.Model;
    using Phundus.Inventory.AvailabilityAndReservation.Repositories;
    using Phundus.Shop.Model;

    public class NhReservationsBasedOnOrdersRepository : IReservationRepository
    {
        public Func<ISession> Session { get; set; }

        private IQueryable<OrderLine> OrderItems
        {
            get { return Session().Query<OrderLine>(); }
        }

        public IEnumerable<Reservation> Find(int articleId, Guid orderItemToExclude)
        {
            var factory = new ReservationFactory();
            var result = new List<Reservation>();

            var query = from oi in OrderItems
                where oi.ArticleShortId.Id == articleId
                      && oi.LineId.Id != orderItemToExclude
                select oi;

            result.AddRange(factory.Create(query));

            return result;
        }

        public IEnumerable<Reservation> Find(ArticleId articleId, Guid orderItemToExclude)
        {
            var factory = new ReservationFactory();
            var result = new List<Reservation>();

            var query = from oi in OrderItems
                        where oi.ArticleId.Id == articleId.Id
                              && oi.LineId.Id != orderItemToExclude
                        select oi;

            result.AddRange(factory.Create(query));

            return result;
        }
    }
}