namespace Phundus.Persistence.IdentityAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using Phundus.IdentityAccess.Organizations.Model;
    using Phundus.IdentityAccess.Organizations.Repositories;

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

        public Organization GetById(OrganizationId organizationId)
        {
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            return GetById(organizationId.Id);
        }

        public Organization FindById(Guid id)
        {
            var query = from o in Entities where o.Id.Id == id select o;
            return query.SingleOrDefault();
        }

        public Organization FindById(OrganizationId organizationId)
        {
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            return FindById(organizationId.Id);
        }

        public Organization FindByName(string name)
        {
            var query = from o in Entities where o.Name == name select o;
            return query.SingleOrDefault();
        }
    }
}