namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;

    public interface IMembershipQueries
    {
        IList<MembershipDto> ByUserId(int userId);
        IList<MembershipDto> ByUserName(string userName);
        IList<MembershipDto> ByOrganizationId(int organizationId);
        IList<MembershipDto> FindByOrganizationId(Guid organizationId);
    }
}