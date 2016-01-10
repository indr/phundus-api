namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Collections.Generic;

    public interface IMembershipApplicationQueries
    {
        IList<MembershipApplicationDto> PendingByOrganizationId(Guid organizationId);
    }
}