namespace Phundus.Persistence.Shop.Repositories
{
    using Common;
    using Common.Domain.Model;
    using Phundus.Shop.Orders.Model;
    using Phundus.Shop.Orders.Repositories;

    public class NhOrderRepository : NhRepositoryBase<Order>, IOrderRepository
    {
        public Order GetById(int id)
        {
            var result = FindById(id);
            if (result == null)
                throw new NotFoundException("Order {0} not found.", id);
            return result;
        }
    }
}