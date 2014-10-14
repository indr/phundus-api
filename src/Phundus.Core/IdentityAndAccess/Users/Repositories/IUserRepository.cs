namespace Phundus.Core.IdentityAndAccess.Users.Repositories
{
    using System.Collections.Generic;
    using Infrastructure;
    using Model;

    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> FindAll();
        User FindByEmail(string email);
        User FindBySessionKey(string sessionKey);
        User FindByValidationKey(string validationKey);

        User FindById(int id);
        User ActiveById(int userId);
        User GetById(int id);

        new int Add(User user);
    }
}