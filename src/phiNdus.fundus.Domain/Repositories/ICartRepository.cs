namespace phiNdus.fundus.Domain.Repositories
{
    using phiNdus.fundus.Domain.Entities;
    using Phundus.Infrastructure;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart FindByCustomer(User customer);
        Cart FindById(int id);
    }
}