using Iesi.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class BasePropertyEntityTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        private readonly DomainPropertyDefinition _namePropertyDefinition = new DomainPropertyDefinition(1, "Name",
                                                                                                         DomainPropertyType
                                                                                                             .Text);

        [Test]
        public void AddProperty_with_the_property_already_added_throws()
        {
            var sut = new BasePropertyEntity();
            sut.AddProperty(_namePropertyDefinition);
            var ex = Assert.Throws<PropertyException>(() => sut.AddProperty(_namePropertyDefinition));
            Assert.That(ex.Message, Is.EqualTo("Property bereits vorhanden."));
        }

        [Test]
        public void AddProperty_with_value()
        {
            var sut = new BasePropertyEntity();
            sut.AddProperty(_namePropertyDefinition, "Value of Name");
            Assert.That(sut.HasProperty(_namePropertyDefinition), Is.True);
            Assert.That(sut.GetPropertyValue(_namePropertyDefinition), Is.EqualTo("Value of Name"));
            Assert.That(sut.GetPropertyValue(_namePropertyDefinition.Id), Is.EqualTo("Value of Name"));
        }

        [Test]
        public void Can_create()
        {
            var sut = new BasePropertyEntity();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PropertyValues, Is.Not.Null);
        }

        [Test]
        public void Can_create_with_PropertyValues()
        {
            var propertyValues = new HashedSet<DomainPropertyValue>();
            var sut = new BasePropertyEntity(propertyValues);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PropertyValues, Is.SameAs(propertyValues));
        }

        [Test]
        public void Can_get_and_set_PropertyValue()
        {
            var sut = new BasePropertyEntity();
            sut.AddProperty(_namePropertyDefinition);
            Assert.That(sut.GetPropertyValue(_namePropertyDefinition), Is.Null);
            sut.SetPropertyValue(_namePropertyDefinition, "Pullover");
            Assert.That(sut.GetPropertyValue(_namePropertyDefinition), Is.EqualTo("Pullover"));
            Assert.That(sut.GetPropertyValue(_namePropertyDefinition.Id), Is.EqualTo("Pullover"));
        }

        [Test]
        public void GetPropertyValue_after_AddProperty_returns_null()
        {
            var sut = new BasePropertyEntity();
            sut.AddProperty(_namePropertyDefinition);
            Assert.That(sut.GetPropertyValue(_namePropertyDefinition), Is.Null);
            Assert.That(sut.GetPropertyValue(_namePropertyDefinition.Id), Is.Null);
        }

        [Test]
        public void GetPropertyValue_without_the_presence_of_the_property_throws()
        {
            var sut = new BasePropertyEntity();
            var ex = Assert.Throws<PropertyException>(() => sut.GetPropertyValue(_namePropertyDefinition));
            Assert.That(ex.Message, Is.EqualTo("Property nicht vorhanden."));

            ex = Assert.Throws<PropertyException>(() => sut.GetPropertyValue(_namePropertyDefinition.Id));
            Assert.That(ex.Message, Is.EqualTo("Property nicht vorhanden."));
        }

        [Test]
        public void HasProperty_after_AddProperty_returns_true()
        {
            var sut = new BasePropertyEntity();
            sut.AddProperty(_namePropertyDefinition);
            Assert.That(sut.HasProperty(_namePropertyDefinition), Is.True);
            Assert.That(sut.HasProperty(_namePropertyDefinition.Id), Is.True);
        }

        [Test]
        public void HasProperty_after_RemoveProperty_returns_false()
        {
            var sut = new BasePropertyEntity();

            sut.AddProperty(_namePropertyDefinition);
            Assert.That(sut.HasProperty(_namePropertyDefinition), Is.True);
            Assert.That(sut.HasProperty(_namePropertyDefinition.Id), Is.True);

            sut.RemoveProperty(_namePropertyDefinition);
            Assert.That(sut.HasProperty(_namePropertyDefinition), Is.False);
            Assert.That(sut.HasProperty(_namePropertyDefinition.Id), Is.False);
        }

        [Test]
        public void HasProperty_returns_false()
        {
            var sut = new BasePropertyEntity();
            Assert.That(sut.HasProperty(_namePropertyDefinition), Is.False);
            Assert.That(sut.HasProperty(_namePropertyDefinition.Id), Is.False);
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var sut = new BasePropertyEntity();
            Assert.That(sut, Is.InstanceOf(typeof (BaseEntity)));
        }

        [Test]
        public void RemoveProperty_without_the_presence_of_the_property_throws()
        {
            var sut = new BasePropertyEntity();
            var ex = Assert.Throws<PropertyException>(() => sut.RemoveProperty(_namePropertyDefinition));
            Assert.That(ex.Message, Is.EqualTo("Property nicht vorhanden."));
        }

        [Test]
        public void SetPropertyValue_after_AddProperty()
        {
            var sut = new BasePropertyEntity();
            sut.AddProperty(_namePropertyDefinition);
            sut.SetPropertyValue(_namePropertyDefinition, "Pullover");
        }

        [Test]
        public void SetPropertyValue_without_the_presence_of_the_property_throws()
        {
            var sut = new BasePropertyEntity();
            var ex = Assert.Throws<PropertyException>(() => sut.SetPropertyValue(_namePropertyDefinition, "Pullover"));
            Assert.That(ex.Message, Is.EqualTo("Property nicht vorhanden."));
        }
    }
}