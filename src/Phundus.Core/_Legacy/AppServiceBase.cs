namespace Phundus.Core
{
    using System;
    using System.Security.Principal;
    using NHibernate;

    public class AppServiceBase
    {
        public IPrincipal Principal{ get; set; }
        public IIdentity Identity { get; set; }

        public Func<ISession> SessionFact { get; set; }
    }
}