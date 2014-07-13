namespace Phundus.Persistence.Legacy.Repositories
{
    using System.Linq;
    using Infrastructure;
    using NHibernate.Linq;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Domain.Repositories;

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