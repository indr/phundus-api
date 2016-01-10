namespace Phundus.Core.IdentityAndAccess.Users.Repositories
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;
    using Infrastructure;
    using Model;

    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> FindAll();
        User FindByEmailAddress(string emailAddress);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        User GetById(UserId userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        User GetById(UserGuid userGuid);

        new int Add(User user);
        User FindActiveByGuid(Guid userId);
        User FindByGuid(Guid userId);

       
    }
}