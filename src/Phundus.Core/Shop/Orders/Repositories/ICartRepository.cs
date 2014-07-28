namespace Phundus.Core.Shop.Orders.Repositories
{
    using IdentityAndAccess.Users.Model;
    using Infrastructure;
    using Model;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart FindByCustomer(User customer);
        Cart FindById(int id);
    }
}