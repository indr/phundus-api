namespace Phundus.Persistence.Inventory.Repositories
{
    using System;
    using System.Collections.Generic;
    using Common.Domain.Model;
    using NHibernate;
    using Phundus.Inventory.AvailabilityAndReservation.Model;
    using Phundus.Inventory.Model.Reservations;
    using Phundus.Shop.Model;
    using Phundus.Shop.Projections;

    public class NhReservationsBasedOnOrdersRepository : IReservationRepository
    {
        public Func<ISession> SessionFactory { get; set; }

        public IEnumerable<Reservation> Find(ArticleId articleId, OrderLineId orderLineIdToExclude)
        {
            var factory = new ReservationFactory();
            var result = new List<Reservation>();

            var query = SessionFactory().QueryOver<OrderLineData>()
                .Where(p => p.ArticleId == articleId.Id);

            if (orderLineIdToExclude != null)
                query = query.AndNot(p => p.LineId == orderLineIdToExclude.Id);


            result.AddRange(factory.Create(query.List()));

            return result;
        }
    }
}