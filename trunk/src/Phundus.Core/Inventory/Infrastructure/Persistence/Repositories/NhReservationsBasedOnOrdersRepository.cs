namespace Phundus.Inventory.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using AvailabilityAndReservation.Model;
    using Common.Domain.Model;
    using Model.Reservations;
    using NHibernate;
    using Shop.Application;

    public class NhReservationsBasedOnOrdersRepository : IReservationRepository
    {
        public Func<ISession> SessionFactory { get; set; }

        public IEnumerable<Reservation> Find(ArticleId articleId, OrderLineId orderLineIdToExclude)
        {
            var factory = new ReservationFactory();
            var result = new List<Reservation>();

            IQueryOver<OrderLineData, OrderLineData> query = SessionFactory().QueryOver<OrderLineData>()
                .Where(p => p.ArticleId == articleId.Id);

            if (orderLineIdToExclude != null)
                query = query.AndNot(p => p.LineId == orderLineIdToExclude.Id);


            result.AddRange(factory.Create(query.List()));

            return result;
        }
    }
}