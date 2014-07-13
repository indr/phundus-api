namespace phiNdus.fundus.Domain.Repositories
{
    using phiNdus.fundus.Domain.Entities;
    using piNuts.phundus.Infrastructure;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart FindByCustomer(User customer);
        Cart FindById(int id);
    }
}