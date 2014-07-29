namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System.Collections.Generic;

    public interface IMembershipQueries
    {
        IList<MembershipDto> ByUserId(int userId);
        IList<MembershipDto> ByUserName(string userName);
        IList<MembershipDto> ByOrganizationId(int organizationId);
    }
}