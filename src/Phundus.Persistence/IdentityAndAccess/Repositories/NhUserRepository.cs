﻿namespace Phundus.Persistence.IdentityAndAccess.Repositories
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using IdentityAccess.Users.Model;
    using IdentityAccess.Users.Repositories;
    using NHibernate.Linq;

    public class NhUserRepository : NhRepositoryBase<User>, IUserRepository
    {
        private IQueryable<User> Users
        {
            get { return Session.Query<User>(); }
        }

        public User GetByGuid(UserId userId)
        {
            var result = FindByGuid(userId.Id);
            if (result == null)
                throw new NotFoundException("User {0} not found.", userId);
            return result;
        }

        public User FindByGuid(Guid userId)
        {
            return Users.SingleOrDefault(p => p.Guid == userId);
        }

        public User FindByEmailAddress(string emailAddress)
        {
            if (emailAddress == null) throw new ArgumentNullException("emailAddress");
            emailAddress = emailAddress.ToLowerInvariant().Trim();

            return Users.Where(u => u.Account.Email == emailAddress).SingleOrDefault();
        }

        public User FindByValidationKey(string validationKey)
        {
            return Users.Where(u => u.Account.ValidationKey == validationKey).SingleOrDefault();
        }
    }
}