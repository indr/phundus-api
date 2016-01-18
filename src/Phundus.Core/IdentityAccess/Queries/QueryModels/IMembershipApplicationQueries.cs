namespace Phundus.IdentityAccess.Queries.QueryModels
{
    using System.Collections.Generic;
    using Common.Domain.Model;

    public interface IMembershipApplicationQueries
    {
        IList<IMembershipApplication> FindPending(CurrentUserId currentUserId, OrganizationGuid organizationGuid);
    }
}