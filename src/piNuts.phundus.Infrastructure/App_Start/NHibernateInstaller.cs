using Castle.Facilities.NHibernate;
using Castle.Transactions;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;

namespace piNuts.phundus.Infrastructure.App_Start
{
    public class NHibernateInstaller : INHibernateInstaller
    {
        private readonly Maybe<IInterceptor> _interceptor;

        public NHibernateInstaller()
        {
            _interceptor = Maybe.None<IInterceptor>();
        }

        #region INHibernateInstaller Members

        public FluentConfiguration BuildFluent()
        {
            var cfg = new Configuration();


            return Fluently.Configure(cfg);
        }

        public void Registered(ISessionFactory factory)
        {
            // Nothing to do here...
        }

        public bool IsDefault
        {
            get { return true; }
        }

        public string SessionFactoryKey
        {
            get { return @"sf.default"; }
        }

        public Maybe<IInterceptor> Interceptor
        {
            get { return _interceptor; }
        }

        #endregion
    }
}