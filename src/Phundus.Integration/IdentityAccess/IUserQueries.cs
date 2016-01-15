namespace Phundus.Integration.IdentityAccess
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Domain.Model;

    public interface IUserQueries
    {
        IUser GetByGuid(Guid guid);
        IUser GetByGuid(UserGuid userGuid);

        IUser FindById(Guid userGuid);

        IUser FindByUsername(string username);
        IList<IUser> Query();
        IUser FindActiveById(Guid userGuid);


        bool IsEmailAddressTaken(string emailAddress);
    }
}