using System.Reflection;
using NHibernate.Cfg;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Mappings
{
    [TestFixture]
    internal class MappingTests
    {
        [Test]
        public void Can_create_dll_from_hibernate_mapping()
        {
            Assert.Ignore();
            var cfg = new Configuration().AddAssembly(Assembly.GetAssembly(typeof (BaseEntity)));
            var export = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
            export.SetOutputFile("ddl.sql");
            export.Execute(true, true, false);
        }
    }
}