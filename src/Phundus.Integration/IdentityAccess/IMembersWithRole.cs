namespace Phundus.Integration.IdentityAccess
{
    using System;
    using System.Collections.Generic;

    public interface IMembersWithRole
    {
        IList<Manager> Manager(Guid tenantId);
    }
}