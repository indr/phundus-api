namespace Phundus.Persistence
{
    using System.Reflection;
    using Castle.Facilities.NHibernate;
    using Castle.Transactions;
    using FluentNHibernate.Cfg;
    using NHibernate;

    public class NHibernateInstaller : INHibernateInstaller
    {
        private readonly Maybe<IInterceptor> _interceptor;

        public NHibernateInstaller()
        {
            _interceptor = Maybe.None<IInterceptor>();
        }

        public FluentConfiguration BuildFluent()
        {
            var cfg = new NHibernate.Cfg.Configuration();
            
            cfg.AddAssembly(Assembly.GetExecutingAssembly());

            return Fluently. Configure(cfg).Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()));
        }

        public void Registered(ISessionFactory factory)
        {
            //
        }

        public bool IsDefault
        {
            get { return true; }
        }

        public string SessionFactoryKey
        {
            get { return "sf.default"; }
        }

        public Maybe<IInterceptor> Interceptor
        {
            get { return _interceptor; }
        }
    }
}