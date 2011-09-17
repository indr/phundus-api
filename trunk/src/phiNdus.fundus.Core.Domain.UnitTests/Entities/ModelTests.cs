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
            var sut =new Model();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Is_derived_from_BasePropertyEntity()
        {
            var sut = new Model();
            Assert.That(sut, Is.InstanceOf(typeof (BasePropertyEntity)));
        }
    }
}