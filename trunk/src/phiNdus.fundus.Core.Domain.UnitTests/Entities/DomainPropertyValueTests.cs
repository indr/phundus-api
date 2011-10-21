using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class DomainPropertyValueTests
    {
        private static FieldValue CreateSut(FieldType type)
        {
            return new FieldValue(new FieldDefinition(type));
        }

        private static FieldValue CreateSut()
        {
            return CreateSut(FieldType.Text);
        }

        [Test]
        public void Can_create()
        {
            var sut = CreateSut(FieldType.Boolean);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PropertyDefinition, Is.Not.Null);
        }

        [Test]
        public void Can_create_with_type_and_value()
        {
            var sut = new FieldValue(new FieldDefinition(FieldType.Text), "Value");
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PropertyDefinition, Is.Not.Null);
            Assert.That(sut.Value, Is.EqualTo("Value"));
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var sut = CreateSut(FieldType.Boolean);
            Assert.That(sut, Is.InstanceOf(typeof(BaseEntity)));
        }

        [Test]
        public void Can_get_and_set_with_BooleanDomainProperty()
        {
            var sut = CreateSut(FieldType.Boolean);
            Assert.That(sut.Value, Is.Null);
            sut.Value = true;
            Assert.That(sut.Value, Is.True);
        }

        [Test]
        public void Can_set_BooleanDomainProperty_to_null()
        {
            var sut = CreateSut(FieldType.Boolean);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_get_and_set_with_TextDomainProperty()
        {
            var sut = CreateSut(FieldType.Text);
            Assert.That(sut.Value, Is.Null);
            sut.Value = "Foo";
            Assert.That(sut.Value, Is.EqualTo("Foo"));
        }

        [Test]
        public void Can_set_TextDomainProperty_to_null()
        {
            var sut = CreateSut(FieldType.Text);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_get_and_set_with_IntegerDomainProperty()
        {
            var sut = CreateSut(FieldType.Integer);
            Assert.That(sut.Value, Is.Null);
            sut.Value = 1;
            Assert.That(sut.Value, Is.EqualTo(1));
        }

        [Test]
        public void Can_set_IntegerDomainProperty_to_null()
        {
            var sut = CreateSut(FieldType.Integer);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_get_and_set_with_DecimalDomainProperty()
        {
            var sut = CreateSut(FieldType.Decimal);
            Assert.That(sut.Value, Is.Null);
            sut.Value = 1.1d;
            Assert.That(sut.Value, Is.EqualTo(1.1d));
        }

        [Test]
        public void Can_set_DecimalDomainProperty_to_null()
        {
            var sut = CreateSut(FieldType.Decimal);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_get_and_set_with_DateTimeDomainProperty()
        {
            var now = DateTime.Now;
            var sut = CreateSut(FieldType.DateTime);
            Assert.That(sut.Value, Is.Null);
            sut.Value = now;
            Assert.That(sut.Value, Is.EqualTo(now));
        }

        [Test]
        public void Can_set_DateTimeDomainProperty_to_null()
        {
            var sut = CreateSut(FieldType.DateTime);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_get_and_set_IsDiscriminator()
        {
            var sut = CreateSut();
            Assert.That(sut.IsDiscriminator, Is.False);
            sut.IsDiscriminator = true;
            Assert.That(sut.IsDiscriminator, Is.True);
        }

        [Test]
        public void Set_IsDiscriminator_sets_value_to_null()
        {
            var sut = CreateSut(FieldType.Text);
            sut.Value = "Text-Value";
            sut.IsDiscriminator = true;
            Assert.That(sut.Value, Is.Null);
        }
    }
}