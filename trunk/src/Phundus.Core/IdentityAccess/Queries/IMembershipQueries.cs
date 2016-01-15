namespace Phundus.IdentityAccess.Queries
{
    using System;
    using System.Collections.Generic;
    using ReadModels;

    public interface IMembershipQueries
    {        
        IList<MembershipDto> ByUserId(Guid userId);
        IList<MembershipDto> ByUserName(string userName);
        IList<MembershipDto> FindByOrganizationId(Guid organizationId);
    }
}