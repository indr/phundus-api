namespace Phundus.Core.IdentityAndAccess.Queries
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
        UserDto GetById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        UserDto GetById(UserId userId);

        UserDto FindById(int userId);
        UserDto FindById(Guid userId);

        UserDto FindByUsername(string username);
        IList<UserDto> All();
        UserDto FindActiveById(Guid userId);

        
    }
}