using System;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Security
{
    public class Secured
    {
        public static SecuredHelper With(Session session)
        {
            Guard.Against<ArgumentNullException>(session == null, "session");

            return new SecuredHelper(session);
        }
    }
}