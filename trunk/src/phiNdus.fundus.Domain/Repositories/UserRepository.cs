using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.Repositories
{
    using piNuts.phundus.Infrastructure.Obsolete;

    public class UserRepository : NHRepository<User>, IUserRepository
    {
        public Func<ISession> SessionFactory { get; set; }

        private IQueryable<User> Users
        {
            get { return Session.Query<User>(); }
        }

        #region IUserRepository Members

        public bool IsSessionFactoryAssigned()
        {
            return SessionFactory != null;
        }

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