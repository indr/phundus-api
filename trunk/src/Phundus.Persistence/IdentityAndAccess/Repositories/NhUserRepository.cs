namespace Phundus.Persistence.IdentityAndAccess.Repositories
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

        public User FindById(int id)
        {
            return Users.Where(p => p.Id == id).SingleOrDefault();
        }

        public User GetById(int id)
        {
            return GetById(new UserId(id));
        }

        public User GetById(UserId userId)
        {
            var result = FindById(userId.Id);
            if (result == null)
                throw new NotFoundException(String.Format("User {0} not found.", userId));
            return result;
        }

        public User GetByGuid(UserGuid userGuid)
        {
            var result = FindByGuid(userGuid.Id);
            if (result == null)
                throw new NotFoundException("User {0} not found.", userGuid);
            return result;
        }

        public new int Add(User user)
        {
            base.Add(user);
            return user.Id;
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