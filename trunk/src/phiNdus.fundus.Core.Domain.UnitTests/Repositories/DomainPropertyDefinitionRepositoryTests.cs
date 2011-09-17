using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Repositories;

namespace phiNdus.fundus.Core.Domain.UnitTests.Repositories
{
    [TestFixture]
    public class DomainPropertyDefinitionRepositoryTests
    {
        [Test]
        public void Can_create()
        {
            IDomainPropertyDefinitionRepository sut = new DomainPropertyDefinitionRepository();
            Assert.That(sut, Is.Not.Null);
        }
    }
}