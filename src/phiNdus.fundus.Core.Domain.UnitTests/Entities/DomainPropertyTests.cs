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
            new FieldDefinition();
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var sut = new FieldDefinition();
            Assert.That(sut, Is.InstanceOf(typeof (Entity)));
        }

        [Test]
        public void Can_get_Name()
        {
            var sut = new FieldDefinition(1, "Name", FieldType.Text);
            Assert.That(sut.Name, Is.EqualTo("Name"));
        }

        [Test]
        public void Can_get_Type()
        {
            var sut = new FieldDefinition(1, "Name", FieldType.Text);
            Assert.That(sut.DataType, Is.EqualTo(FieldType.Text));
        }

        [Test]
        public void Can_get_IsSystemProperty()
        {
            var sut = new FieldDefinition(1, 2, "Name", FieldType.Text, true);
            Assert.That(sut.IsSystemProperty, Is.True);
        }
    }
}