namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System.Collections.Generic;

    public interface IOrganizationQueries
    {
        IEnumerable<OrganizationDto> ByMemberId(int memberId);
        OrganizationDetailDto ById(int id);
        IEnumerable<OrganizationDto> All();
        IEnumerable<OrganizationDto> AllNonFree();
    }
}