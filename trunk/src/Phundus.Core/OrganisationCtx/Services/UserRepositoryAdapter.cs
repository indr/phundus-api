﻿namespace Phundus.Core.OrganisationCtx.Services
{
    using DomainModel;
    using IdentityAndAccessCtx.DomainModel;
    using IdentityAndAccessCtx.Queries;

    public class UserRepositoryAdapter
    {
        private readonly IUserQueries _users;

        public UserRepositoryAdapter(IUserQueries users)
        {
            _users = users;
        }

        public Member ToMember(int id)
        {
            User user = _users.ById(id);
            if (user == null)
                return null;

            return new Member(user.Id, user.FirstName, user.LastName);
        }
    }
}