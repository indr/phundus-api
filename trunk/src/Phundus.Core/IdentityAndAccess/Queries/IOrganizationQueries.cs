namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;

    public interface IOrganizationQueries
    {
        IEnumerable<OrganizationDto> ByMemberId(int memberId);
        OrganizationDetailDto ById(int id);
        OrganizationDetailDto FindById(Guid id);
        IEnumerable<OrganizationDto> All();
        IEnumerable<OrganizationDto> AllNonFree();
    }
}