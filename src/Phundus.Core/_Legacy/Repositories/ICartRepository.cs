namespace Phundus.Core.Repositories
{
    using IdentityAndAccessCtx.DomainModel;
    using Phundus.Core.Entities;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart FindByCustomer(User customer);
        Cart FindById(int id);
    }
}