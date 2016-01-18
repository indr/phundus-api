namespace Phundus.IdentityAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Integration.IdentityAccess;

    public interface IOrganizationQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        IOrganization GetById(Guid organizationId);

        IOrganization FindById(Guid organizationId);

        IEnumerable<IOrganization> All();
    }
}