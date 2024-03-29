﻿namespace Phundus.IdentityAccess.Resources
{
    using System;
    using Application;
    using Castle.Transactions;
    using Common.Resources;

    public interface IUsersResource
    {
        UserData Get(Guid userId);
    }

    public class UsersResource : ApiControllerBase, IUsersResource
    {
        private readonly IUserQueryService _userQueryService;

        public UsersResource(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }

        [Transaction]
        public UserData Get(Guid userId)
        {
            return _userQueryService.FindById(userId);
        }
    }
}