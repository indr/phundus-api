using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class ItemPropertyTests
    {
        [Test]
        public void Can_create()
        {
            new ItemProperty();
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var sut = new ItemProperty();
            Assert.That(sut, Is.InstanceOf(typeof (BaseEntity)));
        }

        [Test]
        public void Can_get_Name()
        {
            var sut = new ItemProperty(1, "Name", ItemPropertyType.Text);
            Assert.That(sut.Name, Is.EqualTo("Name"));
        }

        [Test]
        public void Can_get_Type()
        {
            var sut = new ItemProperty(1, "Name", ItemPropertyType.Text);
            Assert.That(sut.Type, Is.EqualTo(ItemPropertyType.Text));
        }
    }
}