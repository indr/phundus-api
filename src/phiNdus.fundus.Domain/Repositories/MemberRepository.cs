namespace phiNdus.fundus.Domain.Repositories
{
    using System.Collections.Generic;
    using Entities;
    using NHibernate;
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class MemberRepository : RepositoryBase<User>, IMemberRepository
    {
        IQueryOver<User, User> Members
        {
            get { return Session.QueryOver<User>(); }
        }

        public ICollection<User> FindByOrganization(int organizationId)
        {
            var q = Members
                .JoinQueryOver<OrganizationMembership>(m => m.Memberships)
                .Where(om => om.Organization.Id == organizationId);
            return q.List<User>();
        }

        public User FindById(int id)
        {
            return Members.Where(p => p.Id == id).SingleOrDefault();
        }
    }
}