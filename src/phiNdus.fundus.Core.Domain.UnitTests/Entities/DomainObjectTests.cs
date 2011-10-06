using System;
using Castle.MicroKernel.Registration;
using Iesi.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using phiNdus.fundus.TestHelpers;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class DomainObjectTests : MockTestBase<DomainObject>
    {

        protected override void RegisterDependencies(Castle.Windsor.IWindsorContainer container)
        {
            base.RegisterDependencies(container);

            StubPropertyDefinitionRepository = MockRepository.GenerateStub<IDomainPropertyDefinitionRepository>();
            
            container.Register(
                Component.For<IDomainPropertyDefinitionRepository>().Instance(StubPropertyDefinitionRepository));
        }
        
        protected override DomainObject CreateSut()
        {
            StubPropertyValues = new HashedSet<DomainPropertyValue>();
            return new DomainObject(StubPropertyValues);
        }

        private ISet<DomainPropertyValue> StubPropertyValues { get; set; }
        private IDomainPropertyDefinitionRepository StubPropertyDefinitionRepository { get; set; }

        private readonly DomainPropertyDefinition _namePropertyDef =
            new DomainPropertyDefinition(DomainPropertyDefinition.CaptionId, "Name",
                                         DomainPropertyType.Text);

        [Test]
        public void Can_create()
        {
            var sut = new DomainObject();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PropertyValues, Is.Not.Null);
        }

        [Test]
        public void Can_create_with_Id_and_Version()
        {
            var sut = new DomainObject(1, 2);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Id, Is.EqualTo(1));
            Assert.That(sut.Version, Is.EqualTo(2));
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
        public void Can_get_HasChildren()
        {
            Assert.That(Sut.HasChildren, Is.False);
            Sut.AddChild(new DomainObject());
            Assert.That(Sut.HasChildren, Is.True);
        }

        [Test]
        public void GetCaption()
        {
            Assert.That(Sut.Caption, Is.EqualTo(""));
            StubPropertyValues.Add(new DomainPropertyValue(_namePropertyDef, "Name of object"));
            Assert.That(Sut.Caption, Is.EqualTo("Name of object"));
        }

        [Test]
        public void SetCaption()
        {
            StubPropertyDefinitionRepository.Stub(x => x.Get(_namePropertyDef.Id))
                .Return(_namePropertyDef);

            Assert.That(Sut.Caption, Is.EqualTo(""));
            Sut.Caption = "Name of object";
            Assert.That(Sut.Caption, Is.EqualTo("Name of object"));
        }
    }
}