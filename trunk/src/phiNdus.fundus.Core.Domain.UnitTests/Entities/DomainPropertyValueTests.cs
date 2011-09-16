using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class DomainPropertyValueTests
    {

        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void Can_create()
        {
            new DomainPropertyValue();
        }

        [Test]
        public void Is_derived_from_BaseEntity()
        {
            var sut = new DomainPropertyValue();
            Assert.That(sut, Is.InstanceOf(typeof(BaseEntity)));
        }
    }
}