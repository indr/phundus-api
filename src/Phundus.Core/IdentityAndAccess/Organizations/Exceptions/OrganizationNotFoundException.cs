namespace Phundus.Core.IdentityAndAccess.Organizations
{
    using System;

    public class OrganizationNotFoundException : Exception
    {
        public OrganizationNotFoundException(int organizationId) : base(String.Format("Organisation mit der Id {0} konnte nicht gefunden werden.", organizationId))
        {
            
        }
    }
}