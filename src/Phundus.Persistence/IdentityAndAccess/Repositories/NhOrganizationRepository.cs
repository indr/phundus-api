namespace Phundus.Persistence.IdentityAndAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Transactions;
    using Common;
    using Core.IdentityAndAccess.Organizations.Model;
    using Core.IdentityAndAccess.Organizations.Repositories;

    public class NhOrganizationRepository : NhRepositoryBase<Organization>, IOrganizationRepository
    {
        [Transaction]
        public ICollection<Organization> FindAll()
        {
            var query = from o in Entities where o.Name != "Reserved" select o;
            return query.ToList();
        }

        public Organization GetById(Guid id)
        {
            var result = FindById(id);
            if (result == null)
                throw new NotFoundException(String.Format("Organization {0} not found.", id));

            return result;
        }

        public Organization FindById(Guid id)
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