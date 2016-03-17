namespace Phundus.IdentityAccess.Infrastructure.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Transactions;
    using Common;
    using Common.Domain.Model;
    using Common.Infrastructure.Persistence;
    using Model.Organizations;
    using Organizations.Model;

    public class NhOrganizationRepository : NhRepositoryBase<Organization>, IOrganizationRepository
    {
        [Transaction]
        public ICollection<Organization> FindAll()
        {
            IQueryable<Organization> query = from o in Entities where o.Name != "Reserved" select o;
            return query.ToList();
        }

        public Organization GetById(Guid id)
        {
            Organization result = FindById(id);
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
            IQueryable<Organization> query = from o in Entities where o.Id.Id == id select o;
            return query.SingleOrDefault();
        }

        public Organization FindById(OrganizationId organizationId)
        {
            if (organizationId == null) throw new ArgumentNullException("organizationId");
            return FindById(organizationId.Id);
        }

        public Organization FindByName(string name)
        {
            IQueryable<Organization> query = from o in Entities where o.Name == name select o;
            return query.SingleOrDefault();
        }
    }
}