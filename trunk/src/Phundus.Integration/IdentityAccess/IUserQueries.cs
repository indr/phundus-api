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

        IUser FindById(int userId);
        IUser FindById(Guid userId);

        IUser FindByUsername(string username);
        IList<IUser> Query();
        IUser FindActiveById(Guid userId);


        bool IsEmailAddressTaken(string emailAddress);
    }
}