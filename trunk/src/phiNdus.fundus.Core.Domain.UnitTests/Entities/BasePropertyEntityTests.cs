using System;
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

        private readonly DomainProperty _nameProperty = new DomainProperty(1, "Name", DomainPropertyType.Text);

        [Test]
        public void Can_create()
        {
            var sut = new BasePropertyEntity();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Can_get_and_set_PropertyValue()
        {
            var sut = new BasePropertyEntity();
            sut.AddProperty(_nameProperty);
            Assert.That(sut.GetPropertyValue(_nameProperty), Is.Null);
            sut.SetPropertyValue(_nameProperty, "Pullover");
            Assert.That(sut.GetPropertyValue(_nameProperty), Is.EqualTo("Pullover"));
        }

        [Test]
        public void GetPropertyValue_after_AddProperty_returns_null()
        {
            var sut = new BasePropertyEntity();
            sut.AddProperty(_nameProperty);
            var actual = sut.GetPropertyValue(_nameProperty);
            Assert.That(actual, Is.Null);
        }

        [Test]
        public void GetPropertyValue_without_the_presence_of_the_property_throws()
        {
            var sut = new BasePropertyEntity();
            var ex = Assert.Throws<Exception>(() => sut.GetPropertyValue(_nameProperty));
            Assert.That(ex.Message, Is.EqualTo("Property nicht vorhanden."));
        }

        [Test]
        public void HasProperty_after_AddProperty_returns_true()
        {
            var sut = new BasePropertyEntity();
            sut.AddProperty(_nameProperty);
            var actual = sut.HasProperty(_nameProperty);
            Assert.That(actual, Is.True);
        }

        [Test]
        public void HasProperty_returns_false()
        {
            var sut = new BasePropertyEntity();
            var actual = sut.HasProperty(_nameProperty);
            Assert.That(actual, Is.False);
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var sut = new BasePropertyEntity();
            Assert.That(sut, Is.InstanceOf(typeof (BaseEntity)));
        }

        [Test]
        public void SetPropertyValue_after_AddProperty()
        {
            var sut = new BasePropertyEntity();
            sut.AddProperty(_nameProperty);
            sut.SetPropertyValue(_nameProperty, "Pullover");
        }

        [Test]
        public void SetPropertyValue_without_the_presence_of_the_property_throws()
        {
            var sut = new BasePropertyEntity();
            var ex = Assert.Throws<Exception>(() => sut.SetPropertyValue(_nameProperty, "Pullover"));
            Assert.That(ex.Message, Is.EqualTo("Property nicht vorhanden."));
        }
    }
}