namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    using Iesi.Collections.Generic;
    using NUnit.Framework;
    using Phundus.Core.Entities;

    [TestFixture]
    public class FieldedEntityTests
    {
        private readonly FieldDefinition _namePropertyDefinition = new FieldDefinition(1, "Name",
            DataType
                .Text);

        [Test]
        public void AddProperty_with_the_property_already_added_throws()
        {
            var sut = new FieldedEntity();
            sut.AddField(_namePropertyDefinition);
            var ex = Assert.Throws<FieldAlreadyAttachedException>(() => sut.AddField(_namePropertyDefinition));
            Assert.That(ex.Message, Is.EqualTo("Property bereits vorhanden."));
        }

        [Test]
        public void AddProperty_with_value()
        {
            var sut = new FieldedEntity();
            sut.AddField(_namePropertyDefinition, "Value of Name");
            Assert.That(sut.HasField(_namePropertyDefinition), Is.True);
            Assert.That(sut.GetFieldValue(_namePropertyDefinition), Is.EqualTo("Value of Name"));
            Assert.That(sut.GetFieldValue(_namePropertyDefinition.Id), Is.EqualTo("Value of Name"));
        }

        [Test]
        public void Can_create()
        {
            var sut = new FieldedEntity();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.FieldValues, Is.Not.Null);
        }

        [Test]
        public void Can_create_with_Id_and_Version()
        {
            var sut = new FieldedEntity(1, 2);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Id, Is.EqualTo(1));
            Assert.That(sut.Version, Is.EqualTo(2));
        }

        [Test]
        public void Can_create_with_PropertyValues()
        {
            var propertyValues = new HashedSet<FieldValue>();
            var sut = new FieldedEntity(propertyValues);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.FieldValues, Is.SameAs(propertyValues));
        }

        [Test]
        public void Can_get_and_set_PropertyValue()
        {
            var sut = new FieldedEntity();
            sut.AddField(_namePropertyDefinition);
            Assert.That(sut.GetFieldValue(_namePropertyDefinition), Is.Null);
            sut.SetFieldValue(_namePropertyDefinition, "Pullover");
            Assert.That(sut.GetFieldValue(_namePropertyDefinition), Is.EqualTo("Pullover"));
        }

        [Test]
        public void Can_get_and_set_PropertyValue_with_PropertyDefinitionId()
        {
            var sut = new FieldedEntity();
            sut.AddField(_namePropertyDefinition);
            Assert.That(sut.GetFieldValue(_namePropertyDefinition.Id), Is.Null);
            sut.SetFieldValue(_namePropertyDefinition.Id, "Pullover");
            Assert.That(sut.GetFieldValue(_namePropertyDefinition.Id), Is.EqualTo("Pullover"));
        }

        [Test]
        public void GetPropertyValue_after_AddProperty_returns_null()
        {
            var sut = new FieldedEntity();
            sut.AddField(_namePropertyDefinition);
            Assert.That(sut.GetFieldValue(_namePropertyDefinition), Is.Null);
            Assert.That(sut.GetFieldValue(_namePropertyDefinition.Id), Is.Null);
        }

        [Test]
        public void GetPropertyValue_without_the_presence_of_the_property_throws()
        {
            var sut = new FieldedEntity();
            var ex = Assert.Throws<FieldAlreadyAttachedException>(() => sut.GetFieldValue(_namePropertyDefinition));
            Assert.That(ex.Message, Is.EqualTo("Property nicht vorhanden."));

            ex = Assert.Throws<FieldAlreadyAttachedException>(() => sut.GetFieldValue(_namePropertyDefinition.Id));
            Assert.That(ex.Message, Is.EqualTo("Property nicht vorhanden."));
        }

        [Test]
        public void HasProperty_after_AddProperty_returns_true()
        {
            var sut = new FieldedEntity();
            sut.AddField(_namePropertyDefinition);
            Assert.That(sut.HasField(_namePropertyDefinition), Is.True);
            Assert.That(sut.HasField(_namePropertyDefinition.Id), Is.True);
        }

        [Test]
        public void HasProperty_after_RemoveProperty_returns_false()
        {
            var sut = new FieldedEntity();

            sut.AddField(_namePropertyDefinition);
            Assert.That(sut.HasField(_namePropertyDefinition), Is.True);
            Assert.That(sut.HasField(_namePropertyDefinition.Id), Is.True);

            sut.RemoveField(_namePropertyDefinition);
            Assert.That(sut.HasField(_namePropertyDefinition), Is.False);
            Assert.That(sut.HasField(_namePropertyDefinition.Id), Is.False);
        }

        [Test]
        public void HasProperty_returns_false()
        {
            var sut = new FieldedEntity();
            Assert.That(sut.HasField(_namePropertyDefinition), Is.False);
            Assert.That(sut.HasField(_namePropertyDefinition.Id), Is.False);
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var sut = new FieldedEntity();
            Assert.That(sut, Is.InstanceOf(typeof (EntityBase)));
        }

        [Test]
        public void RemoveProperty_without_the_presence_of_the_property_throws()
        {
            var sut = new FieldedEntity();
            var ex = Assert.Throws<FieldAlreadyAttachedException>(() => sut.RemoveField(_namePropertyDefinition));
            Assert.That(ex.Message, Is.EqualTo("Property nicht vorhanden."));
        }

        [Test]
        public void SetPropertyValue_after_AddProperty()
        {
            var sut = new FieldedEntity();
            sut.AddField(_namePropertyDefinition);
            sut.SetFieldValue(_namePropertyDefinition, "Pullover");
        }

        [Test]
        public void SetPropertyValue_without_the_presence_of_the_property_throws()
        {
            var sut = new FieldedEntity();
            var ex =
                Assert.Throws<FieldAlreadyAttachedException>(
                    () => sut.SetFieldValue(_namePropertyDefinition, "Pullover"));
            Assert.That(ex.Message, Is.EqualTo("Property nicht vorhanden."));
        }
    }
}