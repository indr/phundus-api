using System;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Repositories
{
    public class CartRepository : NHRepository<Cart>, ICartRepository
    {
        private IQueryable<Cart> Carts
        {
            get { return Session.Query<Cart>(); }
        }

        public Cart FindByCustomer(User customer)
        {
            var query = from c in Carts
                        where c.Customer.Id == customer.Id
                        select c;
            return query.SingleOrDefault();
        }
    }
}