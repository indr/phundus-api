namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System.Collections.Generic;

    public interface IUserQueries
    {
        UserDto ById(int id);
        UserDto ByUserName(string userName);
        IList<UserDto> All();
    }
}