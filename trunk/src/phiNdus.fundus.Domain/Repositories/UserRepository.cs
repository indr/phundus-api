namespace phiNdus.fundus.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using NHibernate.Linq;
    using piNuts.phundus.Infrastructure;

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private IQueryable<User> Users
        {
            get { return Session.Query<User>(); }
        }

        #region IUserRepository Members

        public ICollection<User> FindAll()
        {
            var query = from u in Users select u;
            return query.ToList();
        }

        public User FindByEmail(string email)
        {
            var query = from u in Users
                        where u.Membership.Email == email
                        select u;
            return query.FirstOrDefault();
        }

        public User FindBySessionKey(string sessionKey)
        {
            var query = from u in Users
                        where u.Membership.SessionKey == sessionKey
                        select u;
            return query.FirstOrDefault();
        }

        public User FindByValidationKey(string validationKey)
        {
            var query = from u in Users
                        where u.Membership.ValidationKey == validationKey
                        select u;
            return query.FirstOrDefault();
        }

        #endregion
    }
}