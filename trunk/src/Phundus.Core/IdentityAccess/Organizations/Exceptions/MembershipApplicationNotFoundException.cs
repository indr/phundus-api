namespace Phundus.IdentityAccess.Organizations.Exceptions
{
    using System;

    public class MembershipApplicationNotFoundException : Exception
    {
        public MembershipApplicationNotFoundException(Guid id) : base(String.Format("Das Beitrittsgesuch mit der Id {0} konnte nicht gefunden werden.", id.ToString("D")))
        {
            
        }
    }
}