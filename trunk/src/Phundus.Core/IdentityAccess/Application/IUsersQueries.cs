namespace Phundus.IdentityAccess.Application
{
    using System;
    using System.Collections.Generic;
    using Projections;

    [Obsolete("Use IUsersResource")]
    public interface IUsersQueries
    {
        UserData GetById(Guid userId);
        UserData FindById(Guid userId);
        UserData FindByUsername(string username);
        UserData FindActiveById(Guid userId);
        bool IsEmailAddressTaken(string emailAddress);
        IList<UserData> Query();
    }
}