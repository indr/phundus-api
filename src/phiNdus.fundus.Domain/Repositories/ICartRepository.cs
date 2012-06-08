using System.Collections.Generic;
using System.Text;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Cart FindByCustomer(User customer);
        Cart FindById(int id);
    }
}
