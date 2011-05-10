using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace phiNdus.fundus.Core.Domain.IntegrationTests
{
    [TestFixture]
    class SchemaExport
    {

        [Test]
        public void Generate_ddl()
        {
            Assert.Ignore();
            //var _factory = new NHibernateUnitOfWorkFactory(new Assembly[] { Assembly.UnsafeLoadFrom("phiNdus.fundus.Core.Domain.dll") });

            
            var cfg = new Configuration().AddAssembly("phiNdus.fundus.Core.Domain");
            //var fact = cfg.Configure().AddAssembly().BuildSessionFactory();
            //fact.
            //var su = new SchemaUpdate(cfg);
            var export = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
            export.SetOutputFile("ddl.sql");
            export.Execute(true, true, false);
            



        }
    }
}
