using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Factories;

namespace phiNdus.fundus.Domain.Repositories
{
    using piNuts.phundus.Infrastructure.Obsolete;

    public class ReservationRepository : IReservationRepository
    {
        private static ISession Session
        {
            get { return UnitOfWork.CurrentSession; }
        }

        private static IQueryable<OrderItem> OrderItems
        {
            get { return Session.Query<OrderItem>(); }
        }

        private IQueryable<ContractItem> ContractItems
        {
            get { return Session.Query<ContractItem>(); }
        }

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
    }

    public interface IReservationRepository
    {
        ICollection<Reservation> Find(Article article);
    }
}