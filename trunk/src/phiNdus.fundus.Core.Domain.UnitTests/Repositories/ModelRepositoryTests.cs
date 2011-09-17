using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Repositories;

namespace phiNdus.fundus.Core.Domain.UnitTests.Repositories
{
    [TestFixture]
    public class ModelRepositoryTests
    {
        [Test]
        public void Can_create()
        {
            IModelRepository sut = new ModelRepository();
            Assert.That(sut, Is.Not.Null);
        }
    }
}