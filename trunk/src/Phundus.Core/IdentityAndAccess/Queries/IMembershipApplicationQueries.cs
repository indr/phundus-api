namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System.Collections.Generic;

    public interface IMembershipApplicationQueries
    {
        IList<MembershipApplicationDto> PendingByOrganizationId(int organizationId);
    }
}