namespace Phundus.IdentityAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using Common;
    using ReadModels;

    public interface IOrganizationQueries
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        OrganizationDetailDto GetById(Guid organizationId);

        OrganizationDetailDto FindById(Guid organizationId);

        IEnumerable<OrganizationDto> All();
    }
}