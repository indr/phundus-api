using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class DomainPropertyValueTests
    {

        [SetUp]
        public void SetUp()
        {
            
        }

        private static DomainPropertyValue CreateSut(DomainPropertyType type)
        {
            return new DomainPropertyValue(new DomainProperty(type));
        }

        [Test]
        public void Can_create()
        {
            CreateSut(DomainPropertyType.Boolean);
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var sut = CreateSut(DomainPropertyType.Boolean);
            Assert.That(sut, Is.InstanceOf(typeof(BaseEntity)));
        }

        [Test]
        public void Can_get_and_set_with_BooleanDomainProperty()
        {
            var sut = new DomainPropertyValue(new DomainProperty(DomainPropertyType.Boolean));
            Assert.That(sut.Value, Is.Null);
            sut.Value = true;
            Assert.That(sut.Value, Is.True);
        }

        [Test]
        public void Can_get_and_set_with_TextDomainProperty()
        {
            var sut = CreateSut(DomainPropertyType.Text);
            Assert.That(sut.Value, Is.Null);
            sut.Value = "Foo";
            Assert.That(sut.Value, Is.EqualTo("Foo"));
        }

        [Test]
        public void Can_get_and_set_with_IntegerDomainProperty()
        {
            var sut = CreateSut(DomainPropertyType.Integer);
            Assert.That(sut.Value, Is.Null);
            sut.Value = 1;
            Assert.That(sut.Value, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_with_DecimalDomainProperty()
        {
            var sut = CreateSut(DomainPropertyType.Decimal);
            Assert.That(sut.Value, Is.Null);
            sut.Value = 1.1d;
            Assert.That(sut.Value, Is.EqualTo(1.1d));
        }

        [Test]
        public void Can_get_and_set_with_DateTimeDomainProperty()
        {
            var now = DateTime.Now;
            var sut = CreateSut(DomainPropertyType.DateTime);
            Assert.That(sut.Value, Is.Null);
            sut.Value = now;
            Assert.That(sut.Value, Is.EqualTo(now));
        }
    }
}