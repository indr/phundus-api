namespace Phundus.Persistence.IdentityAndAccess.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Organizations;
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

        public Organization GetById(int id)
        {
            var result = FindById(id);
            if (result == null)
                throw new OrganizationNotFoundException(id);
            return result;
        }

        public Organization FindByName(string name)
        {
            var query = from o in Entities where o.Name == name select o;
            return query.SingleOrDefault();
        }
    }
}