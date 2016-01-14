namespace Phundus.IdentityAccess.Users.Repositories
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Infrastructure;
    using Model;

    public interface IUserRepository : IRepository<User>
    {
        User FindByEmailAddress(string emailAddress);

        User FindByValidationKey(string validationKey);

        User FindById(int id);

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
        User FindByGuid(Guid userId);
    }
}