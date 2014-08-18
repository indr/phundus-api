namespace Phundus.Core.IdentityAndAccess.Organizations
{
    using System;

    public class MembershipApplicationNotFoundException : Exception
    {
        public MembershipApplicationNotFoundException(Guid id) : base(String.Format("Die Beitrittsanfrage mit der Id {0} konnte nicht gefunden werden.", id.ToString("D")))
        {
            
        }
    }
}