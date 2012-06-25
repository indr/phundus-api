using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.IntegrationTests.Mappings
{
    [TestFixture]
    public class MappingTests
    {
        [Test]
        public void Can_create_ddl_from_hibernate_mapping()
        {
            var cfg = new Configuration().AddAssembly(Assembly.GetAssembly(typeof (EntityBase)));
            var export = new SchemaExport(cfg);
            export.SetOutputFile("Ddl-Generated.sql");
            export.Execute(false, false, false);
        }
    }
}