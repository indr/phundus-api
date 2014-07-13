using NUnit.Framework;
using phiNdus.fundus.Domain.Repositories;

namespace phiNdus.fundus.Domain.UnitTests.Repositories
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