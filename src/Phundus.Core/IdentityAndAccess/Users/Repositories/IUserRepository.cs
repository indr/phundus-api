namespace Phundus.Core.IdentityAndAccess.Users.Repositories
{
    using System.Collections.Generic;
    using Common;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        User GetById(int id);

        new int Add(User user);
    }
}