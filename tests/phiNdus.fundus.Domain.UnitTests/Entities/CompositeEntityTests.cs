using System;
using Castle.MicroKernel.Registration;
using Iesi.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.TestHelpers;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    using piNuts.phundus.Infrastructure;

    [TestFixture]
    public class CompositeEntityTests : UnitTestBase<object>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            StubFieldDefinitionRepository = MockRepository.GenerateStub<IFieldDefinitionRepository>();

            GlobalContainer.Container.Register(Component.For<IFieldDefinitionRepository>()
                .Instance(StubFieldDefinitionRepository));
        }

        protected CompositeEntity CreateSut()
        {
            StubFieldValues = new HashedSet<FieldValue>();
            return new CompositeEntity(StubFieldValues);
        }

        private ISet<FieldValue> StubFieldValues { get; set; }
        private IFieldDefinitionRepository StubFieldDefinitionRepository { get; set; }

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
        public void Can_create_with_FieldValues()
        {
            var fieldValues = new HashedSet<FieldValue>();
            var sut = new CompositeEntity(fieldValues);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.FieldValues, Is.SameAs(fieldValues));
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
            var sut = new CompositeEntity();
            Assert.That(sut.HasChildren, Is.False);
            sut.AddChild(new CompositeEntity());
            Assert.That(sut.HasChildren, Is.True);
        }
    }
}