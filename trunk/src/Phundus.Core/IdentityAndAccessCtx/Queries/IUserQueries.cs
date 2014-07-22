namespace Phundus.Core.IdentityAndAccessCtx.Queries
{
    using System.Collections.Generic;

    public interface IUserQueries
    {
        UserDto ById(int id);
        IList<UserDto> All();
    }
}