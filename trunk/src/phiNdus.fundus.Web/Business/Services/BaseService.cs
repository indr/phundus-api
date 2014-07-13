namespace phiNdus.fundus.Web.Business.Services
{
    using System;
    using System.Security.Principal;
    using NHibernate;

    public class BaseService
    {
        public IPrincipal Principal{ get; set; }
        public IIdentity Identity { get; set; }

        public Func<ISession> SessionFact { get; set; }
    }
}