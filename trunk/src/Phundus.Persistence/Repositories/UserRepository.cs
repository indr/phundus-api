namespace phiNdus.fundus.Domain.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate;
    using NHibernate.Linq;
    using Phundus.Core.Entities;
    using Phundus.Core.Repositories;
    using Phundus.Persistence;

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private IQueryable<User> Users
        {
            get { return Session.Query<User>(); }
        }

        private IQueryOver<User, User> Members
        {
            get { return Session.QueryOver<User>(); }
        }

        #region IUserRepository Members

        public ICollection<User> FindByOrganization(int organizationId)
        {
            IQueryOver<User, OrganizationMembership> q = Members
                .JoinQueryOver<OrganizationMembership>(m => m.Memberships)
                .Where(om => om.Organization.Id == organizationId);
            return q.List<User>();
        }

        public User FindById(int id)
        {
            return Members.Where(p => p.Id == id).SingleOrDefault();
        }

        public ICollection<User> FindAll()
        {
            IQueryable<User> query = from u in Users select u;
            return query.ToList();
        }

        public User FindByEmail(string email)
        {
            IQueryable<User> query = from u in Users
                where u.Membership.Email == email
                select u;
            return query.FirstOrDefault();
        }

        public User FindBySessionKey(string sessionKey)
        {
            IQueryable<User> query = from u in Users
                where u.Membership.SessionKey == sessionKey
                select u;
            return query.FirstOrDefault();
        }

        public User FindByValidationKey(string validationKey)
        {
            IQueryable<User> query = from u in Users
                where u.Membership.ValidationKey == validationKey
                select u;
            return query.FirstOrDefault();
        }

        #endregion
    }
}