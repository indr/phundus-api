namespace Phundus.Integration.IdentityAccess
{
    using System;
    using System.Collections.Generic;

    public interface IUsersQueries
    {
        IUser GetById(Guid userId);
        IUser FindById(Guid userId);
        IUser FindByUsername(string username);
        IUser FindActiveById(Guid userId);
        bool IsEmailAddressTaken(string emailAddress);
        IList<IUser> Query();
    }
}