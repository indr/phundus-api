namespace Phundus.Core.IdentityAndAccess.Users
{
    using System;

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(int userId) : base(String.Format("Der Benutzer mit der Id {0} konnte nicht gefunden werden.", userId))
        {
            throw new NotImplementedException();
        }
    }
}