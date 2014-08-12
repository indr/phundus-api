namespace Phundus.Core.IdentityAndAccess.Organizations
{
    using System;

    public class MemberNotFoundException : Exception
    {
        public MemberNotFoundException()
        {
        }

        public MemberNotFoundException(int id)
            : base(string.Format("Das Mitglied mit der Id {0} konnte nicht gefunden werden.", id))
        {
        }
    }
}