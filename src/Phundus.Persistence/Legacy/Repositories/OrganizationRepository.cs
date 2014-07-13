namespace Phundus.Persistence.Legacy.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Transactions;
    using Infrastructure;
    using NHibernate.Linq;
    using phiNdus.fundus.Domain.Entities;
    using phiNdus.fundus.Domain.Repositories;

    public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
    {
        private IQueryable<Organization> Organizations
        {
            get { return Session.Query<Organization>(); }
        }

        #region IOrganizationRepository Members

        [Transaction]
        public ICollection<Organization> FindAll()
        {
            var query = from o in Organizations where o.Name != "Reserved" select o;
            return query.ToList();
        }

        public Organization FindById(int id)
        {
            var query = from o in Organizations where o.Id == id select o;
            return query.SingleOrDefault();
        }

        public Organization FindByName(string name)
        {
            var query = from o in Organizations where o.Name == name select o;
            return query.SingleOrDefault();
        }

        #endregion
    }
}