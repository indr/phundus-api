namespace Phundus.IdentityAccess.Organizations.Exceptions
{
    using System;

    public class AttemptToLockOneselfException : Exception
    {
        public AttemptToLockOneselfException() : base("Sie können sich nicht selber aus der Organisation sperren.")
        {
            
        }
    }
}