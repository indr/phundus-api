namespace Phundus.Core.ShopCtx
{
    using IdentityAndAccessCtx.DomainModel;
    using Infrastructure;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart FindByCustomer(User customer);
        Cart FindById(int id);
    }
}