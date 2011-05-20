using System;
using System.IO;
using System.Linq;
using NHibernate.Linq;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Repositories
{
    public class UserRepository : NHRepository<User>, IUserRepository
    {
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

        private IQueryable<User> Users
        {
            get { return Session.Query<User>(); }
        }
    }
}