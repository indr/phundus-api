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
            new DomainPropertyDefinition();
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var sut = new DomainPropertyDefinition();
            Assert.That(sut, Is.InstanceOf(typeof (BaseEntity)));
        }

        [Test]
        public void Can_get_Name()
        {
            var sut = new DomainPropertyDefinition(1, "Name", DomainPropertyType.Text);
            Assert.That(sut.Name, Is.EqualTo("Name"));
        }

        [Test]
        public void Can_get_Type()
        {
            var sut = new DomainPropertyDefinition(1, "Name", DomainPropertyType.Text);
            Assert.That(sut.DataType, Is.EqualTo(DomainPropertyType.Text));
        }

        [Test]
        public void Can_get_IsSystemProperty()
        {
            var sut = new DomainPropertyDefinition(1, 2, "Name", DomainPropertyType.Text, true);
            Assert.That(sut.IsSystemProperty, Is.True);
        }
    }
}