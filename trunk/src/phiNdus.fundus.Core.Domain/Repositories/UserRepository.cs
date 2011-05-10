using System.Linq;
using NHibernate.Linq;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain
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

        private IQueryable<User> Users
        {
            get { return Session.Query<User>(); }
        }
    }
}