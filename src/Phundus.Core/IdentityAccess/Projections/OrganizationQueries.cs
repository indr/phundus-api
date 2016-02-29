namespace Phundus.IdentityAccess.Projections
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Cqrs;

    public interface IOrganizationQueries
    {
        IEnumerable<OrganizationData> All();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        OrganizationData GetById(Guid organizationId);

        OrganizationData FindById(Guid organizationId);
    }

    public class OrganizationQueries : QueryBase<OrganizationData>, IOrganizationQueries
    {
        public IEnumerable<OrganizationData> All()
        {
            return QueryOver().OrderBy(p => p.Name).Asc.List();
        }

        public OrganizationData GetById(Guid organizationId)
        {
            var result = FindById(organizationId);
            if (result == null)
                throw new NotFoundException("Organization {0} not found.", organizationId);
            return result;
        }

        public OrganizationData FindById(Guid organizationId)
        {
            return QueryOver().Where(p => p.OrganizationId == organizationId).SingleOrDefault();
        }
    }
}