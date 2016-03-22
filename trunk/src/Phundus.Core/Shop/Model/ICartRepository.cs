namespace Phundus.Shop.Model
{
    using Common.Domain.Model;    

    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetByUserGuid(UserId userId);
        Cart FindByUserGuid(UserId userId);

        void Save(Cart aggregate);
    }
}