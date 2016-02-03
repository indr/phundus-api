namespace Phundus.Persistence
{
    using System.IO;
    using System.Reflection;
    using System.Web;
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
            return Fluently.Configure(new NHibernate.Cfg.Configuration())
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .ExposeConfiguration(WriteConfiguration);
        }

        public void Registered(ISessionFactory factory)
        {
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

        private static void WriteConfiguration(Configuration cfg)
        {
            //var fileName = HostingEnvironment.MapPath(@"~\App_Data\SchemaUpdate.sql") ?? @".\SchemaUpdate.sql";
            var fileName = HttpContext.Current.Server.MapPath(@"~\App_Data\SchemaUpdate.sql");
            
            var writer = new StreamWriter(fileName, false);
            new SchemaUpdate(cfg).Execute(writer.WriteLine, true);
        }
    }
}