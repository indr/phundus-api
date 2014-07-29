namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System.Collections.Generic;

    public interface IMembershipQueries
    {
        IList<MembershipDto> ByMemberId(int memberId);
        IList<MembershipDto> ByOrganizationId(int organizationId);
    }
}