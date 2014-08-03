namespace Phundus.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Inventory.Model;
    using Core.ReservationCtx.Model;
    using Core.ReservationCtx.Repositories;
    using Core.ReservationCtx.Services;
    using Core.Shop.Orders.Model;
    using NHibernate;
    using NHibernate.Linq;
    using Phundus.Core.ReservationCtx;

    public class ReservationRepository : IReservationRepository
    {
        public Func<ISession> Session { get; set; }

        private IQueryable<OrderItem> OrderItems
        {
            get { return Session().Query<OrderItem>(); }
        }

        private IQueryable<ContractItem> ContractItems
        {
            get { return Session().Query<ContractItem>(); }
        }

        #region IReservationRepository Members

        public ICollection<Reservation> Find(Article article)
        {
            var factory = new ReservationFactory();
            var result = new List<Reservation>();

            var query = from oi in OrderItems
                        where oi.Article.Id == article.Id
                        //&& oi.To > DateTime.Today
                        select oi;

            result.AddRange(factory.Create(query));

            return result;
        }

        #endregion
    }
}