namespace Phundus.Core.IdentityAndAccess.Organizations
{
    using System;

    public class AttemptToLockOneselfException : Exception
    {
        public AttemptToLockOneselfException() : base("Sie können sich nicht selber aus der Organisation sperren.")
        {
            
        }
    }
}