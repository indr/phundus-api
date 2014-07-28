namespace Phundus.Persistence.Shop.Repositories
{
    using System.Linq;
    using Core.Shop.Orders.Model;
    using Core.Shop.Orders.Repositories;
    using NHibernate.Linq;

    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        private IQueryable<Cart> Carts
        {
            get { return Session.Query<Cart>(); }
        }

        public Cart FindByCustomer(int userId)
        {
            var query = from c in Carts
                where c.Customer.Id == userId
                select c;
            return query.SingleOrDefault();
        }

        public Cart FindById(int id)
        {
            return ById(id);
        }
    }
}