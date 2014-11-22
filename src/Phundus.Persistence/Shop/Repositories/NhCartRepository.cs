namespace Phundus.Persistence.Shop.Repositories
{
    using System.Linq;
    using Core.Shop.Domain.Model.Ordering;
    using Core.Shop.Orders;
    using Core.Shop.Orders.Model;

    public class NhCartRepository : NhRepositoryBase<Cart>, ICartRepository
    {
        public Cart GetById(object id)
        {
            var result = FindById(id);
            if (result == null)
                throw new CartNotFoundException();
            return result;
        }

        public Cart FindByCustomer(int userId)
        {
            var query = from c in Entities
                where c.Customer.Id == userId
                select c;
            return query.SingleOrDefault();
        }        
    }
}