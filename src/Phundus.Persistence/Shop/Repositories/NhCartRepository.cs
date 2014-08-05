namespace Phundus.Persistence.Shop.Repositories
{
    using System.Linq;
    using Core.Shop.Orders.Model;
    using Core.Shop.Orders.Repositories;

    public class NhCartRepository : NhRepositoryBase<Cart>, ICartRepository
    {
        public Cart FindByCustomer(int userId)
        {
            var query = from c in Entities
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