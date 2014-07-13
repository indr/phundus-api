using NUnit.Framework;
using phiNdus.fundus.Domain.Repositories;

namespace phiNdus.fundus.Domain.UnitTests.Repositories
{
    [TestFixture]
    public class FieldDefinitionRepositoryTests
    {
        [Test]
        public void Can_create()
        {
            IFieldDefinitionRepository sut = new FieldDefinitionRepository();
            Assert.That(sut, Is.Not.Null);
        }
    }
}