namespace Phundus.Core.Repositories
{
    using Phundus.Core.Entities;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart FindByCustomer(User customer);
        Cart FindById(int id);
    }
}