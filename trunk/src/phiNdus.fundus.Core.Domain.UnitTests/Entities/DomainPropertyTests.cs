using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class DomainPropertyTests
    {
        [Test]
        public void Can_create()
        {
            new DomainProperty();
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var sut = new DomainProperty();
            Assert.That(sut, Is.InstanceOf(typeof (BaseEntity)));
        }

        [Test]
        public void Can_get_Name()
        {
            var sut = new DomainProperty(1, "Name", DomainPropertyType.Text);
            Assert.That(sut.Name, Is.EqualTo("Name"));
        }

        [Test]
        public void Can_get_Type()
        {
            var sut = new DomainProperty(1, "Name", DomainPropertyType.Text);
            Assert.That(sut.Type, Is.EqualTo(DomainPropertyType.Text));
        }
    }
}