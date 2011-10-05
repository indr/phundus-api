using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public class OrderRepository : NHRepository<Order>, IOrderRepository
    {
        private IQueryable<Order> Orders
        {
            get { return Session.Query<Order>(); }
        }

        public ICollection<Order> FindPending()
        {
            var query = from o in Orders select o;
            return query.ToList();
        }

        public ICollection<Order> FindApproved()
        {
            var query = from o in Orders select o;
            return query.ToList();
        }

        public ICollection<Order> FindRejected()
        {
            var query = from o in Orders select o;
            return query.ToList();
        }
    }
}