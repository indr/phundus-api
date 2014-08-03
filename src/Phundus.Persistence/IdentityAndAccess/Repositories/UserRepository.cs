namespace phiNdus.fundus.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NHibernate;
    using NHibernate.Linq;
    using Phundus.Core.IdentityAndAccess.Users.Model;
    using Phundus.Core.IdentityAndAccess.Users.Repositories;
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

        public IEnumerable<User> FindByOrganization(int organizationId)
        {
            throw new NotSupportedException();
            //IQueryOver<User, OrganizationMembership> q = Members
            //    .JoinQueryOver<OrganizationMembership>(m => m.Memberships)
            //    .Where(om => om.Organization.Id == organizationId);
            //return q.List<User>();
        }

        public User FindById(int id)
        {
            return Members.Where(p => p.Id == id).SingleOrDefault();
        }

        public User ActiveById(int userId)
        {
            return Members.Where(p => p.Id == userId)
                .And(p => p.Account.IsApproved)
                .And(p => !p.Account.IsLockedOut)
                .SingleOrDefault();
        }

        public IEnumerable<User> FindAll()
        {
            IQueryable<User> query = from u in Users select u;
            return query.ToList();
        }

        public User FindByEmail(string email)
        {
            IQueryable<User> query = from u in Users
                where u.Account.Email == email
                select u;
            return query.FirstOrDefault();
        }

        public User FindBySessionKey(string sessionKey)
        {
            IQueryable<User> query = from u in Users
                where u.Account.SessionKey == sessionKey
                select u;
            return query.FirstOrDefault();
        }

        public User FindByValidationKey(string validationKey)
        {
            IQueryable<User> query = from u in Users
                where u.Account.ValidationKey == validationKey
                select u;
            return query.FirstOrDefault();
        }

        #endregion
    }
}