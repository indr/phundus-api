using System;
using System.Linq;
using NHibernate.Linq;

namespace phiNdus.fundus.Domain.Repositories
{
    using Phundus.Core.IdentityAndAccess.Users.Model;
    using Phundus.Core.Shop.Orders.Model;
    using Phundus.Core.Shop.Orders.Repositories;
    using Phundus.Persistence;

    public class CartRepository : RepositoryBase<Cart>, ICartRepository
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

        public Cart FindById(int id)
        {
            return ById(id);
        }
    }
}