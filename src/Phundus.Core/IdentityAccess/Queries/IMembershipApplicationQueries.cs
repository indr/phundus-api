namespace Phundus.IdentityAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using ReadModels;

    public interface IMembershipApplicationQueries
    {
        IList<MembershipApplicationDto> PendingByOrganizationId(Guid organizationId);
    }
}