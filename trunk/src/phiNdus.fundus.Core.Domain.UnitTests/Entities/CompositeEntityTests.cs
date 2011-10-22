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
    public class CompositeEntityTests : MockTestBase<CompositeEntity>
    {

        protected override void RegisterDependencies(Castle.Windsor.IWindsorContainer container)
        {
            base.RegisterDependencies(container);

            StubPropertyDefinitionRepository = MockRepository.GenerateStub<IFieldDefinitionRepository>();
            
            container.Register(
                Component.For<IFieldDefinitionRepository>().Instance(StubPropertyDefinitionRepository));
        }
        
        protected override CompositeEntity CreateSut()
        {
            StubPropertyValues = new HashedSet<FieldValue>();
            return new CompositeEntity(StubPropertyValues);
        }

        private ISet<FieldValue> StubPropertyValues { get; set; }
        private IFieldDefinitionRepository StubPropertyDefinitionRepository { get; set; }

        [Test]
        public void Can_create()
        {
            var sut = new CompositeEntity();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.FieldValues, Is.Not.Null);
        }

        [Test]
        public void Can_create_with_Id_and_Version()
        {
            var sut = new CompositeEntity(1, 2);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Id, Is.EqualTo(1));
            Assert.That(sut.Version, Is.EqualTo(2));
        }

        [Test]
        public void Can_create_with_PropertyValues()
        {
            var propertyValues = new HashedSet<FieldValue>();
            var sut = new CompositeEntity(propertyValues);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.FieldValues, Is.SameAs(propertyValues));
        }

        [Test]
        public void AddChild()
        {
            var sut = new CompositeEntity();
            var child = new CompositeEntity();
            var actual = sut.AddChild(child);
            Assert.That(actual, Is.True);
            Assert.That(sut.Children, Has.Count.EqualTo(1));
        }

        [Test]
        public void AddChild_sets_Parent()
        {
            var parent = new CompositeEntity();
            var sut = new CompositeEntity();
            parent.AddChild(sut);
            Assert.That(sut.Parent, Is.SameAs(parent));
        }

        [Test]
        public void Can_get_HasChildren()
        {
            Assert.That(Sut.HasChildren, Is.False);
            Sut.AddChild(new CompositeEntity());
            Assert.That(Sut.HasChildren, Is.True);
        }
    }
}