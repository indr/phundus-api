namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using Common;

    public interface IOrganizationQueries
    {
        IEnumerable<OrganizationDto> ByMemberId(int memberId);        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        OrganizationDetailDto GetById(Guid organizationId);
        
        OrganizationDetailDto FindById(Guid organizationId);
        IEnumerable<OrganizationDto> All();
        IEnumerable<OrganizationDto> AllNonFree();
    }
}