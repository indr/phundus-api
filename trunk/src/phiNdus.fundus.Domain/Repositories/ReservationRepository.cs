namespace phiNdus.fundus.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate;
    using NHibernate.Linq;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Domain.Factories;

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

    public interface IReservationRepository
    {
        ICollection<Reservation> Find(Article article);
    }
}