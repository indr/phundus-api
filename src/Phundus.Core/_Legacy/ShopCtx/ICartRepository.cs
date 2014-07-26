namespace Phundus.Core.ShopCtx
{
    using IdentityAndAccess.Users.Model;
    using Infrastructure;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart FindByCustomer(User customer);
        Cart FindById(int id);
    }
}