namespace Phundus.IdentityAccess.Projections
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.Querying;

    public interface IOrganizationQueries
    {
        OrganizationData GetById(Guid organizationId);
        OrganizationData FindById(Guid organizationId);
        IEnumerable<OrganizationData> Query();
    }

    public class OrganizationQueries : QueryBase<OrganizationData>, IOrganizationQueries
    {
        public OrganizationData GetById(Guid organizationId)
        {
            var result = FindById(organizationId);
            if (result == null)
                throw new NotFoundException("Organization {0} not found.", organizationId);
            return result;
        }

        public OrganizationData FindById(Guid organizationId)
        {
            return SingleOrDefault(p => p.OrganizationId == organizationId);
        }

        public IEnumerable<OrganizationData> Query()
        {
            return QueryOver().OrderBy(p => p.Name).Asc.List();
        }
    }
}