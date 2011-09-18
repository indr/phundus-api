using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Repositories;

namespace phiNdus.fundus.Core.Domain.UnitTests.Repositories
{
    [TestFixture]
    public class ArticleRepositoryTests
    {
        [Test]
        public void Can_create()
        {
            IArticleRepository sut = new ArticleRepository();
            Assert.That(sut, Is.Not.Null);
        }
    }
}