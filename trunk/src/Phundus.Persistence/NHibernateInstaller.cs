namespace Phundus.Persistence
{
    using System.IO;
    using System.Reflection;
    using System.Web.Hosting;
    using Castle.Facilities.NHibernate;
    using Castle.Transactions;
    using FluentNHibernate.Cfg;
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;

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


            return Fluently.Configure(cfg)
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .ExposeConfiguration(WriteConfiguration);
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

        private void WriteConfiguration(Configuration cfg)
        {
            using (var writer = new StreamWriter(HostingEnvironment.MapPath(@"~\App_Data\SchemaUpdate.sql"), false))
                new SchemaUpdate(cfg).Execute(sa => writer.WriteLine(sa), false);
        }
    }
}