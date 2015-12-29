namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System.Collections.Generic;
    using Common;

    public interface IUserQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        UserDto GetById(int id);

        UserDto ByUserName(string userName);
        IList<UserDto> All();
    }
}