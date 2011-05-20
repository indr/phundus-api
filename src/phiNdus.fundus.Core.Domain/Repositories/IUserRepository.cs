using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByEmail(string email);
        User FindBySessionKey(string sessionKey);
    }
}