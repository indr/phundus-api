using System.Collections.Generic;
using System.Text;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.Repositories
{
    using piNuts.phundus.Infrastructure.Obsolete;

    public interface ICartRepository : IRepository<Cart>
    {
        Cart FindByCustomer(User customer);
        Cart FindById(int id);
    }
}
