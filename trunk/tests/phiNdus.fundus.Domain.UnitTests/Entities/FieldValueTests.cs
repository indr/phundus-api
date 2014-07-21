namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    using System;
    using NUnit.Framework;
    using Phundus.Core.Ddd;
    using Phundus.Core.InventoryCtx;

    [TestFixture]
    public class FieldValueTests
    {
        private static FieldValue CreateSut(DataType type)
        {
            return new FieldValue(new FieldDefinition(type));
        }

        private static FieldValue CreateSut()
        {
            return CreateSut(DataType.Text);
        }

        [Test]
        public void Can_create()
        {
            FieldValue sut = CreateSut(DataType.Boolean);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.FieldDefinition, Is.Not.Null);
        }

        [Test]
        public void Can_create_with_type_and_value()
        {
            var sut = new FieldValue(new FieldDefinition(DataType.Text), "Value");
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.FieldDefinition, Is.Not.Null);
            Assert.That(sut.Value, Is.EqualTo("Value"));
        }

        [Test]
        public void Can_get_and_set_IsDiscriminator()
        {
            FieldValue sut = CreateSut();
            Assert.That(sut.IsDiscriminator, Is.False);
            sut.IsDiscriminator = true;
            Assert.That(sut.IsDiscriminator, Is.True);
        }

        [Test]
        public void Can_get_and_set_with_BooleanField()
        {
            FieldValue sut = CreateSut(DataType.Boolean);
            Assert.That(sut.Value, Is.Null);
            sut.Value = true;
            Assert.That(sut.Value, Is.True);
        }

        [Test]
        public void Can_get_and_set_with_DateTimeField()
        {
            DateTime now = DateTime.Now;
            FieldValue sut = CreateSut(DataType.DateTime);
            Assert.That(sut.Value, Is.Null);
            sut.Value = now;
            Assert.That(sut.Value, Is.EqualTo(now));
        }

        [Test]
        public void Can_get_and_set_with_DecimalField()
        {
            FieldValue sut = CreateSut(DataType.Decimal);
            Assert.That(sut.Value, Is.Null);
            sut.Value = 1.1d;
            Assert.That(sut.Value, Is.EqualTo(1.1d));
        }

        [Test]
        public void Can_get_and_set_with_IntegerField()
        {
            FieldValue sut = CreateSut(DataType.Integer);
            Assert.That(sut.Value, Is.Null);
            sut.Value = 1;
            Assert.That(sut.Value, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_with_TextField()
        {
            FieldValue sut = CreateSut(DataType.Text);
            Assert.That(sut.Value, Is.Null);
            sut.Value = "Foo";
            Assert.That(sut.Value, Is.EqualTo("Foo"));
        }

        [Test]
        public void Can_set_BooleanField_to_null()
        {
            FieldValue sut = CreateSut(DataType.Boolean);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_set_DateTimeField_to_null()
        {
            FieldValue sut = CreateSut(DataType.DateTime);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_set_DecimalField_to_null()
        {
            FieldValue sut = CreateSut(DataType.Decimal);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_set_IntegerField_to_null()
        {
            FieldValue sut = CreateSut(DataType.Integer);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_set_TextField_to_null()
        {
            FieldValue sut = CreateSut(DataType.Text);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            FieldValue sut = CreateSut(DataType.Boolean);
            Assert.That(sut, Is.InstanceOf(typeof (EntityBase)));
        }

        [Test]
        public void Set_IsDiscriminator_sets_value_to_null()
        {
            FieldValue sut = CreateSut(DataType.Text);
            sut.Value = "Text-Value";
            sut.IsDiscriminator = true;
            Assert.That(sut.Value, Is.Null);
        }
    }
}