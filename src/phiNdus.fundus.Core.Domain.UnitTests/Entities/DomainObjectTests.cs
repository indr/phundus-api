using System;
using Iesi.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.TestHelpers;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class DomainObjectTests : MockTestBase<DomainObject>
    {
        protected override DomainObject CreateSut()
        {
            StubPropertyValues = new HashedSet<DomainPropertyValue>();
            return new DomainObject(StubPropertyValues);
        }

        private ISet<DomainPropertyValue> StubPropertyValues { get; set; }

        private readonly DomainPropertyDefinition _textPropertyDef =
            new DomainPropertyDefinition(DomainPropertyDefinition.NameId, "Name",
                                         DomainPropertyType.Text);

        [Test]
        public void Can_create()
        {
            var sut = new DomainObject();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PropertyValues, Is.Not.Null);
        }

        [Test]
        public void Can_create_with_PropertyValues()
        {
            var propertyValues = new HashedSet<DomainPropertyValue>();
            var sut = new DomainObject(propertyValues);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PropertyValues, Is.SameAs(propertyValues));
        }

        [Test]
        public void Is_derived_from_BasePropertyEntity()
        {
            var sut = new DomainObject();
            Assert.That(sut, Is.InstanceOf(typeof (BasePropertyEntity)));
        }

        [Test]
        public void AddChild()
        {
            var sut = new DomainObject();
            var child = new DomainObject();
            sut.AddChild(child);
            Assert.That(sut.Children, Has.Count.EqualTo(1));
        }

        [Test]
        public void AddChild_sets_Parent()
        {
            var parent = new DomainObject();
            var sut = new DomainObject();
            parent.AddChild(sut);
            Assert.That(sut.Parent, Is.SameAs(parent));
        }

        [Test]
        public void GetName()
        {
            Assert.That(Sut.Name, Is.EqualTo(""));
            StubPropertyValues.Add(new DomainPropertyValue(_textPropertyDef, "Name of object"));
            Assert.That(Sut.Name, Is.EqualTo("Name of object"));
        }
    }
}