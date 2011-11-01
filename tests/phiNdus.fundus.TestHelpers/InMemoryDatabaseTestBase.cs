using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;
using Environment = NHibernate.Cfg.Environment;

namespace phiNdus.fundus.TestHelpers
{
    public class InMemoryDatabaseTestBase : IDisposable
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;

        public InMemoryDatabaseTestBase()
        {
            if (_configuration == null)
            {
                _configuration = new Configuration()
                    .SetProperty(Environment.ReleaseConnections, "on_close")
                    .SetProperty(Environment.Dialect, typeof (SQLiteDialect).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionDriver, typeof (SQLite20Driver).AssemblyQualifiedName)
                    .SetProperty(Environment.ConnectionString, "data source=:memory:")
                    .SetProperty(Environment.ProxyFactoryFactoryClass,
                                 typeof (ProxyFactoryFactory).AssemblyQualifiedName)
                    .AddAssembly(Assembly.GetAssembly(typeof (Entity)));

                _sessionFactory = _configuration.BuildSessionFactory();
            }

            Session = _sessionFactory.OpenSession();

            new SchemaExport(_configuration).Execute(true, true, false, Session.Connection, Console.Out);

            var unitOfWork = new NHibernateUnitOfWorkAdapter(null, Session, null);
            UnitOfWork.RegisterGlobalUnitOfWork(unitOfWork);
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