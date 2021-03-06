﻿namespace Phundus.IdentityAccess.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using Common;
    using Common.Domain.Model;
    using Common.Infrastructure.Persistence;
    using Model.Users;
    using NHibernate.Linq;
    using Users.Model;

    public class NhUserRepository : NhRepositoryBase<User>, IUserRepository
    {
        private IQueryable<User> Users
        {
            get { return Session.Query<User>(); }
        }

        public User GetById(UserId userId)
        {
            User result = FindByGuid(userId.Id);
            if (result == null)
                throw new NotFoundException("User {0} not found.", userId);
            return result;
        }

        public void Save(User user)
        {
            Session.Update(user);
        }

        public User FindByGuid(Guid userId)
        {
            return Users.SingleOrDefault(p => p.UserId.Id == userId);
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