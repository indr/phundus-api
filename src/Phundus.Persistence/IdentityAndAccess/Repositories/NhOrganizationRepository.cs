namespace Phundus.Persistence.IdentityAndAccess.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations.Model;
    using Core.IdentityAndAccess.Organizations.Repositories;
    using NHibernate.Linq;
    using Persistence;

    public class NhOrganizationRepository : NhRepositoryBase<Organization>, IOrganizationRepository
    {
        [Transaction]
        public ICollection<Organization> FindAll()
        {
            var query = from o in Entities where o.Name != "Reserved" select o;
            return query.ToList();
        }

        public Organization FindById(int id)
        {
            var query = from o in Entities where o.Id == id select o;
            return query.SingleOrDefault();
        }

        public Organization FindByName(string name)
        {
            var query = from o in Entities where o.Name == name select o;
            return query.SingleOrDefault();
        }
    }
}