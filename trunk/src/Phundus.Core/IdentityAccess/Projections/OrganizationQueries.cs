namespace Phundus.IdentityAccess.Projections
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Cqrs;
    using Integration.IdentityAccess;

    public interface IOrganizationQueries
    {
        IEnumerable<IOrganization> All();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        IOrganization GetById(Guid organizationId);

        IOrganization FindById(Guid organizationId);
    }

    public class OrganizationQueries : QueryBase<OrganizationData>, IOrganizationQueries
    {
        public IEnumerable<IOrganization> All()
        {
            return QueryOver().OrderBy(p => p.Name).Asc.List();
        }

        public IOrganization GetById(Guid organizationId)
        {
            var result = FindById(organizationId);
            if (result == null)
                throw new NotFoundException("Organization {0} not found.", organizationId);
            return result;
        }

        public IOrganization FindById(Guid organizationId)
        {
            return QueryOver().Where(p => p.OrganizationId == organizationId).SingleOrDefault();
        }
    }
}