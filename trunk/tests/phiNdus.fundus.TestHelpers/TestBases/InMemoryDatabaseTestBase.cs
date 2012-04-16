using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Rhino.Commons;
using Environment = NHibernate.Cfg.Environment;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    public class SQLiteInMemoryTestingConnectionProvider : NHibernate.Connection.DriverConnectionProvider
    {
        public static System.Data.IDbConnection Connection = null;

        public override System.Data.IDbConnection GetConnection()
        {
            if (Connection == null)
                Connection = base.GetConnection();

            return Connection;
        }

        public override void CloseConnection(System.Data.IDbConnection conn) { }
    }

    public class InMemoryDatabaseTestBase : TestBase, IDisposable
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;
        private readonly NHibernateUnitOfWorkFactory _factory;

        //[TestFixtureSetUp]
        public InMemoryDatabaseTestBase(Assembly assemblyContainingMappings)
        {
            if (_configuration == null)
            {
                _configuration = new Configuration()
                    //.SetProperty(Environment.ReleaseConnections, "on_close")
                    .SetProperty(Environment.Dialect, typeof(SQLiteDialect).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionDriver, typeof(SQLite20Driver).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionProvider, typeof(SQLiteInMemoryTestingConnectionProvider).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionString, "data source=:memory:")
                    // Standard in NHibernate 3.2 ist die eigene ProxyFactory-Factory.
                    //.SetProperty(Environment.ProxyFactoryFactoryClass,
                    //             typeof (ProxyFactoryFactory).AssemblyQualifiedName)
                    .AddAssembly(assemblyContainingMappings);

                _sessionFactory = _configuration.BuildSessionFactory();
                
            }

            if (_sessionFactory == null)
                throw new Exception("Die Session-Factory konnte anscheinend nicht erzeugt werden! o.O");

            _factory = new NHibernateUnitOfWorkFactory();
            _factory.RegisterSessionFactory(_configuration, _sessionFactory);


            var session = _sessionFactory.OpenSession();
            new SchemaExport(_configuration).Execute(true, true, false, session.Connection, null);
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            IoC.Container.Register(Component.For<IUnitOfWorkFactory>()
                                       .Instance(_factory));
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (SQLiteInMemoryTestingConnectionProvider.Connection != null)
                    SQLiteInMemoryTestingConnectionProvider.Connection.Close();

            SQLiteInMemoryTestingConnectionProvider.Connection = null;
            //Session.Dispose();
        }

        #endregion
    }
}