using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Repositories;

namespace phiNdus.fundus.Core.Domain.UnitTests.Repositories
{
    [TestFixture]
    public class DomainPropertyRepositoryTests
    {
        [Test]
        public void Can_create()
        {
            IDomainPropertyRepository sut = new DomainPropertyRepository();
            Assert.That(sut, Is.Not.Null);
        }
    }
}