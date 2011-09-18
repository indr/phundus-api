using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class ArticleTests
    {
        [Test]
        public void Can_create()
        {
            var sut = new Article();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Is_derived_from_DomainObject()
        {
            var sut = new Article();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf(typeof (DomainObject)));
        }
    }
}