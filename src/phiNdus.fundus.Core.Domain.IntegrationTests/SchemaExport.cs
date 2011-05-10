using NHibernate.Cfg;
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
            var cfg = new Configuration().AddAssembly("phiNdus.fundus.Core.Domain");
            var export = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
            export.SetOutputFile("ddl.sql");
            export.Execute(true, true, false);
        }
    }
}
