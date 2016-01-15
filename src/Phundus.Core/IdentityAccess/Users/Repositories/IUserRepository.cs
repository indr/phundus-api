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

        User FindByGuid(Guid userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        User GetByGuid(UserGuid userGuid);
    }
}