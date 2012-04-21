using System.Collections.Generic;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        ICollection<User> FindAll();
        User FindByEmail(string email);
        User FindBySessionKey(string sessionKey);
        User FindByValidationKey(string validationKey);
    }
}