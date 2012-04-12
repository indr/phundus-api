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
    public class InMemoryDatabaseTestBase : TestBase, IDisposable
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;
        
        //[TestFixtureSetUp]
        public InMemoryDatabaseTestBase(Assembly assemblyContainingMappings)
        {
            if (_configuration == null)
            {
                _configuration = new Configuration()
                    //.SetProperty(Environment.ReleaseConnections, "on_close")
                    .SetProperty(Environment.Dialect, typeof(SQLiteDialect).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionDriver, typeof(SQLite20Driver).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionString, "data source=:memory:")
                    // Standard in NHibernate 3.2 ist die eigene ProxyFactory-Factory.
                    //.SetProperty(Environment.ProxyFactoryFactoryClass,
                    //             typeof (ProxyFactoryFactory).AssemblyQualifiedName)
                    .AddAssembly(assemblyContainingMappings);

                _sessionFactory = _configuration.BuildSessionFactory();
                
            }

            if (_sessionFactory == null)
                throw new Exception("Die Session-Factory konnte anscheinend nicht erzeugt werden! o.O");

            Session = _sessionFactory.OpenSession();
                new SchemaExport(_configuration).Execute(true, true, false, Session.Connection, null);
        }

        private ISession Session { get; set; }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var factory = new NHibernateUnitOfWorkFactory();
            factory.RegisterSessionFactory(_configuration, _sessionFactory);
            

            IoC.Container.Register(Component.For<IUnitOfWorkFactory>()
                                       .Instance(factory));
        }

        #region IDisposable Members

        public void Dispose()
        {
            Session.Dispose();
        }

        #endregion
    }
}