namespace Phundus.Integration.IdentityAccess
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public interface IUserQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        IUser GetById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        IUser GetById(UserId userId);

        IUser GetByGuid(Guid guid);
        IUser GetByGuid(UserGuid userGuid);

        IUser FindById(int userId);
        IUser FindById(Guid userGuid);

        IUser FindByUsername(string username);
        IList<IUser> Query();
        IUser FindActiveById(Guid userGuid);


        bool IsEmailAddressTaken(string emailAddress);
    }
}