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
            IFieldDefinitionRepository sut = new FieldDefinitionRepository();
            Assert.That(sut, Is.Not.Null);
        }
    }
}