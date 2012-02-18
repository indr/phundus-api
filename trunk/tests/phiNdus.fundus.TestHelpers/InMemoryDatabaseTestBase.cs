using System;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using Environment = NHibernate.Cfg.Environment;

namespace phiNdus.fundus.TestHelpers
{
    public class InMemoryDatabaseTestBase : UnitTestBase, IDisposable
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;

        public InMemoryDatabaseTestBase(Assembly assemblyContainingMappings)
        {
            if (_configuration == null)
            {
                _configuration = new Configuration()
                    .SetProperty(Environment.ReleaseConnections, "on_close")
                    .SetProperty(Environment.Dialect, typeof (SQLiteDialect).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionDriver, typeof (SQLite20Driver).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionString, "data source=:memory:")
                    // Standard in NHibernate 3.2 ist die eigene ProxyFactory-Factory.
                    //.SetProperty(Environment.ProxyFactoryFactoryClass,
                    //             typeof (ProxyFactoryFactory).AssemblyQualifiedName)
                    .AddAssembly(assemblyContainingMappings);

                _sessionFactory = _configuration.BuildSessionFactory();
            }

            Session = _sessionFactory.OpenSession();

            new SchemaExport(_configuration).Execute(true, true, false, Session.Connection, Console.Out);
        }

        protected ISession Session { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            Session.Dispose();
        }

        #endregion
    }
}