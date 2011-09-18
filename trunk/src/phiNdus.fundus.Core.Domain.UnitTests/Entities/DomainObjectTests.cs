using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class DomainObjectTests
    {
        [Test]
        public void Can_create()
        {
            var sut =new DomainObject();
            Assert.That(sut, Is.Not.Null);
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
    }
}