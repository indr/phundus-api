using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class ItemTests
    {
        [Test]
        public void Can_create()
        {
            new Item();
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var item = new Item();
            Assert.That(item, Is.InstanceOf(typeof (BaseEntity)));
        }
    }
}