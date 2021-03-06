﻿namespace Phundus.IdentityAccess.Model.Users
{
    using System;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Users.Model;

    public interface IUserRepository : IRepository<User>
    {
        User FindByEmailAddress(string emailAddress);

        User FindByValidationKey(string validationKey);

        User FindByGuid(Guid userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        User GetById(UserId userId);

        void Save(User user);
    }
}