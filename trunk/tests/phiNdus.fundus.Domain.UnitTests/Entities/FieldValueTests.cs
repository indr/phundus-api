﻿using System;
using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.UnitTests.Entities
{
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
            var sut = CreateSut(DataType.Boolean);
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
        public void Is_derived_from_BaseEntity()
        {
            var sut = CreateSut(DataType.Boolean);
            Assert.That(sut, Is.InstanceOf(typeof(Entity)));
        }

        [Test]
        public void Can_get_and_set_with_BooleanField()
        {
            var sut = CreateSut(DataType.Boolean);
            Assert.That(sut.Value, Is.Null);
            sut.Value = true;
            Assert.That(sut.Value, Is.True);
        }

        [Test]
        public void Can_set_BooleanField_to_null()
        {
            var sut = CreateSut(DataType.Boolean);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_get_and_set_with_TextField()
        {
            var sut = CreateSut(DataType.Text);
            Assert.That(sut.Value, Is.Null);
            sut.Value = "Foo";
            Assert.That(sut.Value, Is.EqualTo("Foo"));
        }

        [Test]
        public void Can_set_TextField_to_null()
        {
            var sut = CreateSut(DataType.Text);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_get_and_set_with_IntegerField()
        {
            var sut = CreateSut(DataType.Integer);
            Assert.That(sut.Value, Is.Null);
            sut.Value = 1;
            Assert.That(sut.Value, Is.EqualTo(1));
        }

        [Test]
        public void Can_set_IntegerField_to_null()
        {
            var sut = CreateSut(DataType.Integer);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_get_and_set_with_DecimalField()
        {
            var sut = CreateSut(DataType.Decimal);
            Assert.That(sut.Value, Is.Null);
            sut.Value = 1.1d;
            Assert.That(sut.Value, Is.EqualTo(1.1d));
        }

        [Test]
        public void Can_set_DecimalField_to_null()
        {
            var sut = CreateSut(DataType.Decimal);
            sut.Value = null;
            Assert.That(sut.Value, Is.Null);
        }

        [Test]
        public void Can_get_and_set_with_DateTimeField()
        {
            var now = DateTime.Now;
            var sut = CreateSut(DataType.DateTime);
            Assert.That(sut.Value, Is.Null);
            sut.Value = now;
            Assert.That(sut.Value, Is.EqualTo(now));
        }

        [Test]
        public void Can_set_DateTimeField_to_null()
        {
            var sut = CreateSut(DataType.DateTime);
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
            var sut = CreateSut(DataType.Text);
            sut.Value = "Text-Value";
            sut.IsDiscriminator = true;
            Assert.That(sut.Value, Is.Null);
        }
    }
}