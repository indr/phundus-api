namespace Phundus.Common.Infrastructure.Persistence.Installers
{
    using System.IO;
    using System.Reflection;
    using System.Web;
    using Castle.Facilities.NHibernate;
    using Castle.Transactions;
    using FluentNHibernate.Cfg;
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Event;
    using NHibernate.Tool.hbm2ddl;
    using Notifications;

    public class NHibernateInstaller : INHibernateInstaller
    {
        private static string _fileName;
        private readonly Maybe<IInterceptor> _interceptor;

        public NHibernateInstaller() : this(null)
        {
        }

        public NHibernateInstaller(string cfgOutputFileName)
        {
            _interceptor = Maybe.None<IInterceptor>();
            _fileName = cfgOutputFileName ?? HttpContext.Current.Server.MapPath(@"~\App_Data\SchemaUpdate.sql");
        }

        public FluentConfiguration BuildFluent()
        {
            var result = Fluently.Configure(new Configuration())
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly());
                    m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof (ProcessedNotificationTrackerStore)));
                })
                .ExposeConfiguration(cfg =>
                {
                    ChangeExposedConfiguration(cfg);
                    WriteConfiguration(cfg);
                });
            return result;
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

        public static void ChangeExposedConfiguration(Configuration cfg)
        {
            cfg.AppendListeners(ListenerType.PostCommitInsert,
                new IPostInsertEventListener[] {new PostCommitPublishNotificationListener()});
        }

        private static void WriteConfiguration(Configuration cfg)
        {
            using (var writer = new StreamWriter(_fileName, false))
            {
                new SchemaUpdate(cfg).Execute(writer.WriteLine, true);
            }
        }
    }

    public class NHibernateSchemaUpdateWriter
    {
    }
}