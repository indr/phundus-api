using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class ModelTests
    {
        [Test]
        public void Can_create()
        {
            new Model();
        }

        [Test]
        public void Is_derived_from_BasePropertyEntity()
        {
            var model = new Model();
            Assert.That(model, Is.InstanceOf(typeof (BasePropertyEntity)));
        }
    }
}